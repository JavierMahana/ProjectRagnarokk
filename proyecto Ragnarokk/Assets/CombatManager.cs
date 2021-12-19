using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;


//ahora elementos de display pueden estar aca pero en el futuro moverlos.
public class CombatManager : MonoBehaviour
{
    public GameObject Background;
    public GameObject CombatCanvas;
    public GameObject EnemyCombatCanvas;
    public CombatDescriptor CombatDescriptor;
    public CombatIconManager IconManager;

    public GameObject PrefabActionButton;
    public GameObject PrefabWeaponButton;
    public GameObject PrefabConsumibleButton;
    public GameObject PrefabDamageTypeButton;
    public GameObject PrefabSynergyButton;

    public TextMeshProUGUI PanelDescriptor;
    public GameObject PanelDMGtypes;
    public GameObject PanelSynergies;

    public GameObject PanelForActions;
    public GameObject FighterClickButton;

    private List<GameObject> AllButtonsInDescriptorPanel = new List<GameObject>();

    public List<GameObject> AllButtonsInPanel = new List<GameObject>();


    public GameObject PermanentCanvas;


    [Range(0.0f, 1.0f)]
    public float playerFightersX;

    [Range(0.0f, 1.0f)]
    public float playerFightersMinY;
    [Range(0.0f, 1.0f)]
    public float playerFightersMaxY;


    [Range(0.0f, 1.0f)]
    public float enemyXColoumn1;

    [Range(0.0f, 1.0f)]
    public float enemyMinY_Coloumn1;
    [Range(0.0f, 1.0f)]
    public float enemyMaxY_Coloumn1;


    [Range(0.0f, 1.0f)]
    public float enemyXColoumn2;

    [Range(0.0f, 1.0f)]
    public float enemyMinY_Coloumn2;
    [Range(0.0f, 1.0f)]
    public float enemyMaxY_Coloumn2;


    [HideInInspector]
    public bool initialized;
    //esta es la base vacia de cualquier fighter.
    [HideInInspector]
    public GameObject FighterBasePrefab;

    //LUCHADORES
    [HideInInspector]
    public List<Fighter> AllCombatFighters = new List<Fighter>(); //Lista que almacenará a los luchadores del combate actual

    //Luchadores aliados  
    public List<Fighter> PlayerFighters = new List<Fighter>();
    List<Fighter> AlivePlayerFighters = new List<Fighter>();

    int PartyMaxHP;
    int PartyCurrentHP;
    bool PartyIsFine; //Se inicializa en true si el combate empieza con el HP general sobre el 75%.

    //Luchadores enemigos
    public List<Fighter> EnemyFighters = new List<Fighter>();
    List<Fighter> AliveEnemyFighters = new List<Fighter>();

    int HordeMaxHP;
    int HordeCurrentHP;
    bool HordeIsFine;

    Fighter EnemyToDestroy;

    //public List<Fighter> AllAliveFighters = new List<Fighter>(); 
    [HideInInspector]
    public Fighter ActiveFighter;

    const int DefenseValue = 10; //Tal vez sea mejor definir un multiplicador en vez de una suma?

    [HideInInspector]
    public FighterSelect CurrentPlayerButton;
    Vector2 OriginalButtonPos;

    sbyte ActiveFighterIndex = -1;

    [HideInInspector]
    public bool TurnInProcess;

    sbyte CombatState = 0;

    bool FleeActionSelected = false;
    bool OnFlee = false;

    int SynergyDeterminant;

    private bool isCrit;
    private bool canFlee;
    private bool isFinalRoom;

    public int round;

    //VARIABLES DE ACCIÓN DE UN TURNO
    /// <summary>
    /// Se utilizan para las condiciones de continuación de la corrutina TurnAction, y para el cálculo de daño en el método Fight.
    /// Se pensaba hacerlas variables locales, pero el método que llama a la corrutina (Update) debe tener acceso a dichas variables para
    /// modificarlas, de otro modo, la corrutina no continuaría nunca.
    /// </summary>
    [HideInInspector]
    public string Action = null;
    //Variables de ataque
    [HideInInspector]
    public Weapon AttackWeapon = null;
    [HideInInspector]
    public int AttackWeaponIndex;
    [HideInInspector]
    public Consumible SelectedConsumible = null;
    [HideInInspector]
    public Fighter Target = null;
    [HideInInspector]
    public bool Confirm = false; //Cuando sea true, la acción se debe llevar a cabo
    [HideInInspector]
    public bool Annulment = false; //Se usa cada vez que se quiere retroceder en la selección de algo

    [HideInInspector]
    //attack done podría cambiarse a ActionDone
    public bool ActionDone = false;

    //Comparador de rapidez para ordenar la lista de luchadores
   
    public void ClearPanelDescriptor()
    {
        foreach (GameObject button in AllButtonsInDescriptorPanel)
        {
            Destroy(button);
        }
        AllButtonsInDescriptorPanel.Clear();
        PanelDescriptor.text = "";
    }

    public void SetlDescriptorText(string descripcion)
    {
        PanelDescriptor.text = descripcion;
    }

    public void AddDamageTypeButton(CombatType damageType)
    {
        //damage type
        GameObject button = Instantiate(PrefabDamageTypeButton);
        button.GetComponent<Button_DamageType>().SetButton(damageType);

        button.transform.SetParent(PanelDMGtypes.transform, false);
        AllButtonsInDescriptorPanel.Add(button);

        //AddSynergyButton(damageType);

    }

    public void AddSynergyButton(CombatType damageType, Fighter f)
    {
        // se añaden los botones de sinergías
        int cantidad = 0;

        foreach (CombatState targetState in f.States)
        {
            if (damageType.Sinergias.Contains(targetState))
            {
                cantidad++;
            }
            else if (damageType.AntiSinergias.Contains(targetState))
            {
                cantidad--;
            }
        }
        

        int total = Mathf.Abs(cantidad);
        Debug.Log($"Cantidad sinergias de Arma escogida contra el enemigo: {cantidad}");
        if (total > 0)
        {
            for (int i = 0; i < total; i++)
            {
                GameObject button = Instantiate(PrefabSynergyButton);

                if(cantidad < 0) { button.GetComponent<SynergyButton>().SynergyIcon.color = Color.black; }

                button.transform.SetParent(PanelSynergies.transform, false);
                AllButtonsInDescriptorPanel.Add(button);
            }
        }
             
    }

    public void FillWithAttackWeapon()
    {
        if(AttackWeapon != null)
        {
            ClearPanelDescriptor();
            AddDamageTypeButton(AttackWeapon.TipoDeDañoQueAplica);
            string description = $" Precisión: {AttackWeapon.BaseAccuracy} \n Daño Base: {AttackWeapon.BaseDamage} \n Crítico: {AttackWeapon.BaseCriticalRate} \n Enfriamiento: {AttackWeapon.BaseCooldown}";
            SetlDescriptorText(description);
        }
    }
    public int SpeedComparer(Fighter f1, Fighter f2)
    {
        int speed1 = f1.Speed;
        int speed2 = f2.Speed;

        return -speed1.CompareTo(speed2); //Signo menos para ordenar de mayor a menor
    }

    public void InitCombatScene(CombatEncounter encounter)
    {
        canFlee = encounter.CanEscape;
        //isFinalRoom
        CombatDescriptor = gameObject.GetComponent<CombatDescriptor>();
        IconManager = gameObject.GetComponent<CombatIconManager>();


        // limpia la lista de enemigos y sus botones más los botones aliados
        // antes de instanciar los del nuevo encuentro
        GameManager.Instance.Enemies.Clear();
        GameManager.Instance.EnemyButtons.Clear();
        GameManager.Instance.PlayerButtons.Clear();

        GameManager.Instance.ConfirmationClick = false;


        int j = 0;
        //Se obtienen los luchadores del jugador
        foreach (PlayerFighter pf in GameManager.Instance.PlayerFighters)
        {
            float t = (((float)3 - 1) - j) / Mathf.Max(3 - 1, 1);

            float viewPortY = Mathf.Lerp(playerFightersMinY, playerFightersMaxY, t);
            Vector3 viewPortPos = new Vector3(playerFightersX, viewPortY, Camera.main.nearClipPlane);

            var worldPos = Camera.main.ViewportToWorldPoint(viewPortPos);
            pf.transform.position = worldPos;



            Fighter ally = pf.gameObject.GetComponent<Fighter>();


            PlayerFighters.Add(ally);
            PartyMaxHP += ally.MaxHP;
            PartyCurrentHP += ally.CurrentHP;

            /*
            string m = ally.Name;
            foreach(CombatState state in ally.States)
            {
                m += (" " + state.Name);
            }
            Debug.Log(m);
            */

            #region Player Buttons
            var playerButton = Instantiate(FighterClickButton);

            RectTransform rectTransform = playerButton.GetComponent<RectTransform>();

            var pFighterComp = pf.GetComponent<Fighter>();

            playerButton.GetComponent<FighterSelect>().Fighter = pFighterComp;
            playerButton.transform.SetParent(PermanentCanvas.transform, true);
            playerButton.GetComponent<Button>().interactable = false;

            var minAnchorPoint = pf.transform.position + (pFighterComp.Size.x / 2) * Vector3.left;
            var maxAnchorPoint = pf.transform.position + (pFighterComp.Size.x / 2) * Vector3.right + pFighterComp.Size.y * 0.75f * Vector3.up;
            Vector2 viewportPointMin = Camera.main.WorldToViewportPoint(minAnchorPoint);
            Vector2 viewportPointMax = Camera.main.WorldToViewportPoint(maxAnchorPoint);

            rectTransform.anchorMin = viewportPointMin;
            rectTransform.anchorMax = viewportPointMax;

            rectTransform.anchoredPosition = Vector2.zero;
            #endregion

            GameManager.Instance.PlayerButtons.Add(playerButton.GetComponent<FighterSelect>());

            j++;
        }
        PartyIsFine = (PartyCurrentHP >= PartyMaxHP * 0.75);
        foreach(Fighter pf in PlayerFighters)
        {
            if(pf.CurrentHP > 0) { AlivePlayerFighters.Add(pf); }
        }



        #region Creación de enemigos y posicionamiento
        //Se crean los enemigos
        int enemyCount = encounter.ListOfEncounterEnemies.Count;
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 worldPos;
            int coloumn1MaxEnemyCount = 3;

            if (i < coloumn1MaxEnemyCount)
            {
                float t = (float)i / coloumn1MaxEnemyCount;//((float)coloumn1MaxEnemyCount - i) / coloumn1MaxEnemyCount;//t=1 cuando i = 0; t=0 cuando i = enemyCount-1
                Debug.Log($"VALOR T: {t}");
                float viewPortY = Mathf.Lerp(enemyMinY_Coloumn1, enemyMaxY_Coloumn1, t);
                Vector3 viewPortPos = new Vector3(enemyXColoumn1, viewPortY, Camera.main.nearClipPlane);

                worldPos = Camera.main.ViewportToWorldPoint(viewPortPos);

            }
            else
            {
                int coloumn2Count = enemyCount - coloumn1MaxEnemyCount;
                int k = i - coloumn1MaxEnemyCount;
                float t = (float)k / coloumn2Count;//(((float)coloumn2Count - k) / coloumn2Count);



                float viewPortY = Mathf.Lerp(enemyMinY_Coloumn2, enemyMaxY_Coloumn2, t);
                Vector3 viewPortPos = new Vector3(enemyXColoumn2, viewPortY, Camera.main.nearClipPlane);

                worldPos = Camera.main.ViewportToWorldPoint(viewPortPos);
            }

            var enemyData = encounter.ListOfEncounterEnemies[i];
            CreateEnemy(enemyData, worldPos);

        }
        #endregion

        HordeCurrentHP = HordeMaxHP;
        HordeIsFine = true;
        AliveEnemyFighters.AddRange(EnemyFighters);

        //Se unen los luchadores en una lista
        AllCombatFighters.AddRange(PlayerFighters);
        AllCombatFighters.AddRange(EnemyFighters);

        foreach(Fighter fighter in AllCombatFighters)
        {
            fighter.IsDefending = false;
        }

        //Los luchadores se ordenan por velocidad
        AllCombatFighters.Sort(SpeedComparer);
        #region ANALIZAR A FUTURO
        //No sé cómo funciona el método Sort por debajo, pero es posible que, al comparar dos valores iguales, no cambie el orden
        //de los elementos, provocando que los luchadores de igual velocidad tengan siempre el mismo orden de turnos. De hecho, los aliados atacan
        //antes que los enemigos de igual velocidad, probablemente porque los luchadores del jugador son añadidos a la lista primero.
        //Si se quisiera aleatorizar el orden de turnos para luchadores igual de rápidos, una solución podría ser modificar el método SpeedComparer
        //para que éste decida aleatoriamente el orden, lo cual se traduce en nunca retornar un 0.
        //Sin embargo, esta lista no está siendo modificada, por lo que el orden definido al iniciar un combate será permanente en cada ciclo de
        //turnos. Si se quisiera ir un paso más allá, podría reordenarse la lista cada vez que acabe un ciclo de turnos, permitiendo a todos los
        //luchadores de igual velocidad tener la oportunidad de actuar primero. Aunque no sé si llamar al método Sort repetidas veces sea óptimo.
        #endregion

        initialized = true;
    }

    private void CreateEnemy(FighterData data, Vector3 position)
    {
        position.z = 0;
        var enemyGameObject = Instantiate(FighterBasePrefab, position, Quaternion.identity);
        enemyGameObject.GetComponentInChildren<SpriteRenderer>().transform.localScale  = data.size;

        GameManager.Instance.SetDataToFighterGO(enemyGameObject, data);
        Fighter enemy = enemyGameObject.GetComponent<Fighter>();

        if (!enemy.reversed)
        {
            enemy.reversed = true;
            enemyGameObject.transform.localScale = new Vector3(-1,1,1);
        }

        var enemyButton = Instantiate(FighterClickButton);

        RectTransform rectTransform = enemyButton.GetComponent<RectTransform>();



        enemyButton.GetComponent<FighterSelect>().Fighter = enemyGameObject.GetComponent<Fighter>();
        enemyButton.transform.SetParent(EnemyCombatCanvas.transform, true);


        var minAnchorPoint = enemyGameObject.transform.position + (enemy.Size.x / 2) * Vector3.left;
        var maxAnchorPoint = enemyGameObject.transform.position + (enemy.Size.x / 2) * Vector3.right + enemy.Size.y * 0.75f * Vector3.up;
        Vector2 viewportPointMin = Camera.main.WorldToViewportPoint(minAnchorPoint);
        Vector2 viewportPointMax = Camera.main.WorldToViewportPoint(maxAnchorPoint);

        rectTransform.anchorMin = viewportPointMin;
        rectTransform.anchorMax = viewportPointMax;

        
        rectTransform.anchoredPosition = Vector2.zero;

        GameManager.Instance.Enemies.Add(enemyGameObject.GetComponent<Fighter>());
        GameManager.Instance.EnemyButtons.Add(enemyButton.GetComponent<FighterSelect>());

        //Se llena una lista con los enemigos recién creados
        
        EnemyFighters.Add(enemy);
        HordeMaxHP += enemy.MaxHP;
    }

    public void ShowActionCanvas(bool show)
    {
        CombatCanvas.GetComponent<Canvas>().enabled = show;
        CombatCanvas.SetActive(show);
    }

    public void ShowEnemyInteractableButton(bool show)
    {
        foreach (FighterSelect button in GameManager.Instance.EnemyButtons)
        {
            //button.selfBbutton.interactable = show;
        }
    }



    //call backs unity
    private void Awake()
    {
        initialized = false;

        TurnInProcess = false;

        PartyMaxHP = 0;
        PartyCurrentHP = 0;
        PartyIsFine = false;

        HordeMaxHP = 0;
        HordeCurrentHP = 0;
        HordeIsFine = false;
    }
    private void Start()
    {
        round = 0;
        GameManager.Instance.ShowPlayerFighters(true);      

        InitCombatScene(GameManager.Instance.currentEncounter);
        ResetWeaponCooldowns();
        //ShowFighterCanvas(false);
        GameManager.Instance.CheckOnLoadScene();
    }
    
    private void Update()
    {
        //casi funcional, falta que esto se chequea justo antes del incio del turno del jugador
       // if (!ActionDone) { CheckActivateActionCanvas(); }
        
        if (initialized != true)
            return;

        //Si no hay un turno en curso, se comprobará el estado del combate, e iniciará un nuevo turno, dentro de una corrutina.
        if(!TurnInProcess  &&  CombatState == 0)
        {
            CombatState = CheckCombatState();
            //El combate aún no termina, inicia el siguiente turno.
            if(CombatState == 0)
            {
                //Debug.Log("Ejecutar turno");
                TurnInProcess = true;

                //ShowFighterCanvas(false);
                CleanPanelSelecion();

                if(EnemyToDestroy != null)
                {
                    Destroy(EnemyToDestroy.gameObject);
                    EnemyToDestroy = null;
                }

                FindNextActiveFighter();
                StartCoroutine(TurnAction());
            }
            else if(CombatState > 0)
            {
                StartCoroutine(Victory());
            }
            else
            {
                var sceneChanger = FindObjectOfType<SceneChanger>();
                sceneChanger.ChangeScene("Defeat");
                Debug.Log("DERROTA");
            }
        }

        //Finaliza el combate por huida
        if(OnFlee)
        {
            RemoveAllCombatStates();
            ResetWeaponCooldowns();

            foreach (Fighter pf in PlayerFighters)
            {
                pf.IsDefending = false;
            }

            var sceneChanger = FindObjectOfType<SceneChanger>();
            sceneChanger.ChangeScene("Exploration");
        }

        // constantemente se revisa si es que se va activar el botón para seleccionar enemigos
        ShowEnemyInteractableButton(GameManager.Instance.ConfirmationClick);

    }

    private IEnumerator Victory()
    {
        RemoveAllCombatStates();
        ResetWeaponCooldowns();

        foreach (Fighter pf in PlayerFighters)
        {
            pf.IsDefending = false;
        }

        string victoryHopeChange = HopeManager.Instance.ChangeHope(2, "Cambio por victoria");
        string victoryDesc = "¡Has ganado! ";
        victoryDesc += victoryHopeChange;
        CombatDescriptor.Clear();
        CombatDescriptor.AddTextLine(victoryDesc, 1.5f);

        yield return null;
        yield return new WaitUntil(() => CombatDescriptor.TextIsEmpty);

        var sceneChanger = FindObjectOfType<SceneChanger>();
        //sceneChanger.ChangeScene("Victory");
        //Debug.Log("VICTORIA");


        //al final del juego no se da exp.
        if (GameManager.Instance.InFloorEnd &&
            GameManager.Instance.FloorToLoadInFloorEnd == FloorToLoad.VICTORY)
        {
            sceneChanger.ChangeScene("Victory");
        }
        else if (ExperiencePanelManager.GetFightersThatWillGainExp().Count >= 1)
        {
            sceneChanger.ChangeScene("CombatVictoryScene");
        }
        else
        {
            sceneChanger.LoadExplorationScene();
        }

        
    }

    public void MoveActivePlayerButton(bool moveFoward)
    {
        if (moveFoward)
        {
            var position = (Vector2)ActiveFighter.transform.position;
            RectTransform rectTransform = CurrentPlayerButton.GetComponent<RectTransform>();
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(position);


            var minAnchorPoint = ActiveFighter.transform.position + (ActiveFighter.Size.x / 2) * Vector3.left;
            var maxAnchorPoint = ActiveFighter.transform.position + (ActiveFighter.Size.x / 2) * Vector3.right + ActiveFighter.Size.y*0.75f * Vector3.up;
            Vector2 viewportPointMin = Camera.main.WorldToViewportPoint(minAnchorPoint);
            Vector2 viewportPointMax = Camera.main.WorldToViewportPoint(maxAnchorPoint);

            rectTransform.anchorMin = viewportPointMin;
            rectTransform.anchorMax = viewportPointMax;


            rectTransform.anchoredPosition = Vector2.zero;

        }
        else
        {
            CurrentPlayerButton.transform.position = OriginalButtonPos;
        }


    }
    public void MovePanel()
    {
        var position = (Vector2)ActiveFighter.transform.position;
        RectTransform rectTransform = PanelForActions.GetComponent<RectTransform>();
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(position);

        rectTransform.anchorMin = viewportPoint;
        rectTransform.anchorMax = viewportPoint;

        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void FindNextActiveFighter()
    {
        //Se busca en la lista de luchadores al siguiente luchador vivo más rápido.
        do
        {
            ActiveFighterIndex++;
            if (ActiveFighterIndex >= AllCombatFighters.Count)
            {
                ActiveFighterIndex = 0; //No se sale de la lista
                StartNewTurnCycle();
            }
            ActiveFighter = AllCombatFighters[ActiveFighterIndex];
        } while (ActiveFighter.CurrentHP <= 0); //Comprueba que esté vivo.

        GameManager.Instance.PlayerOnTurn = ActiveFighter.gameObject; //¿PlayerOnTurn está en desuso?

        //Debug.Log(ActiveFighter.Name);
    }

    public void StartNewTurnCycle()
    {
        round++;
        RemoveAllCombatStates();
        DiminishWeaponCooldowns();
    }

    private IEnumerator TurnAction()
    {
        //Si el luchador estaba defendiéndose, sale de ese estado
        if(ActiveFighter.IsDefending)
        {
            ActiveFighter.Defense -= DefenseValue;
            ActiveFighter.IsDefending = false;
        }
        Debug.Log("En defensa:");
        foreach (Fighter f in AllCombatFighters)
        {
            if (f.IsDefending) { Debug.Log(f.Name + " (" + f.Defense + ")"); }
        }

        var iniPos = ActiveFighter.transform.position;

        const float pauseTime = 0.8f; //Tiempo estándar usado para pausas breves

        yield return null; //Es posible que la necesidad de esta línea se deba a que se consulta por la variable TextIsEmpty, la cual se actualiza en Update, en vez de consultar directamente el tamaño de la lista de TextLines.
        yield return new WaitUntil(() => CombatDescriptor.TextIsEmpty);
        CombatDescriptor.ShowFighterInTurn(ActiveFighter, IsPlayerFighter(ActiveFighter));

        

        //TURNO DE UN ALIADO
        if (IsPlayerFighter(ActiveFighter))
        {
            PanelDescriptor.text = "Escoge una acción!";

            //Debug.Log("Turno Aliado");
            AttackWeapon = null;
            SelectedConsumible = null;

            // activa canvas de acciones 
           // ShowFighterCanvas(true);
            ActionSelection();
            ShowActionCanvas(true);
            ClearPanelDescriptor();

            // setea el boton del aliado actual 
            foreach (FighterSelect button in GameManager.Instance.PlayerButtons)
            {
                if (button.Fighter == ActiveFighter)
                {
                    CurrentPlayerButton = button;
                    OriginalButtonPos = CurrentPlayerButton.transform.position;
                }
            }

            // mueve al mujador y su botón
            ActiveFighter.transform.position += new Vector3((float)2.5, 0, 0);
            MoveActivePlayerButton(true);

            #region obsoleto
            /*
               if (false) //Este código quedará obsoleto?
               {
                   //SELECCIÓN DE LA ACCIÓN
                   do
                   {
                       Action = null;
                       //La corrutina se detendrá hasta que se defina una acción en Update.
                       Debug.Log("Escogiendo Acción...");
                       yield return new WaitUntil(() => Action != null);

                       switch (Action)
                       {
                           case "Attack":
                               //SELECCIÓN DEL ARMA
                               do
                               {
                                   Debug.Log("ACCIÓN: " + Action);
                                   AttackWeapon = null;
                                   //Se esperará a definir un arma, o a cancelar la acción
                                   Debug.Log("Escogiendo Arma...");
                                   yield return new WaitUntil(() => AttackWeapon != null || Annulment);

                                   if (Annulment)
                                   {
                                       Action = null; //Se cancela la acción
                                       Annulment = false;
                                       continue; //NO se llevará a cabo la elección de un objetivo
                                   }

                                   //SELECCIÓN DEL OBJETIVO
                                   do
                                   {
                                       Debug.Log("ACCIÓN: " + Action + " | ARMA: " + AttackWeapon.Name);
                                       Annulment = false;
                                       Target = null;
                                       Debug.Log("Escogiendo Objetivo...");
                                       yield return new WaitUntil(() => Target != null || Annulment);

                                       if (Annulment)
                                       {
                                           AttackWeapon = null; //Se anula la elección de arma
                                           Annulment = false;
                                           continue; //NO se pedirá la confirmación de una acción
                                       }

                                       Debug.Log("ACCIÓN: " + Action + " | ARMA: " + AttackWeapon.Name + " | OBJETIVO: " + Target.Name);
                                       //Se solicita confirmación de la acción.
                                       Debug.Log("Confirmando...");
                                       Confirm = false;
                                       yield return new WaitUntil(() => Confirm || Annulment);
                                   } while ((Target == null && AttackWeapon != null) || Annulment); //Se da cuando el jugador quiere reseleccionar el objetivo (puede que sea suficiente consultar por Annulment)
                               } while (AttackWeapon == null && Action != null); //Se da cuando el jugador quiere cambiar de arma

                               break;
                       }
                   } while (Action == null); //No acaba el turno sin una acción a ejecutar, lo que implica que no hay turnos saltables.

                   if(Action.Equals("Attack")) 
                   { //Fight();
                     }
               }
               */
            #endregion

            //No continúa hasta que la acción ha sido descrita por completo
            yield return new WaitUntil(() => ActionDone && CombatDescriptor.TextIsEmpty);

            //Por seguridad se nulifican variables de ataque
            AttackWeapon = null;
            Target = null;

            if (FleeActionSelected)
            {
                OnFlee = true;
                yield break; //Se interrumpe la corrutina. El turno NO se declara como terminado. Esto es para prevenir que se inicie un nuevo turno.
            }
            ActionDone = false;
            MoveActivePlayerButton(false);
        }
        //TURNO DE UN ENEMIGO
        else
        {
            //Debug.Log("Turno Enemigo");
            //ShowFighterCanvas(false);

            ActiveFighter.transform.position += new Vector3((float)-2.5, 0, 0);

            yield return new WaitForSeconds(pauseTime);

            //Selección de arma
            List<int> indexes = new List<int>();
            for(int i = 0;i<4;i++) //Se podria comprobar si existe el arma del index antes de añadir, y asi evitar el ciclo while despues
            {
                indexes.Add(i); 
            }
            while(AttackWeapon == null)
            {
                int index = indexes[Random.Range(0, indexes.Count)];
                AttackWeapon = ActiveFighter.Weapons[index];
                indexes.Remove(index);
            }
            
            Target = AlivePlayerFighters[Random.Range(0, AlivePlayerFighters.Count)];
            //Debug.Log("PRE-TARGET: " + Target.Name);

            // la variable button utiliza el boton de accion por default, sin embargo
            // debe ser reemplazado por el botón del jugador que será el target antes de llamar la funcion Fight
            // de no ser reemplazado en el siguiente while, es porque uno de los botones no fue asignado correctamente
            // a alguno de los 3 jugadores aliados
            FighterSelect targetButton = null;

            foreach(FighterSelect button in GameManager.Instance.PlayerButtons)
            {
                if (button.Fighter == Target && button.Fighter.CurrentHP > 0)
                {
                    targetButton = button;
                    //Debug.Log("Boton Aliado encontrado por el enemigo");
                }
            }          
           
            Fight(targetButton);

            //No continúa hasta que la acción ha sido descrita por completo
            yield return new WaitUntil(() => CombatDescriptor.TextIsEmpty);
            
            //Por seguridad se nulifican variables de ataque
            AttackWeapon = null;
            Target = null;
        }

        yield return new WaitForSeconds(pauseTime);

        ActiveFighter.transform.position = iniPos;
        // indica que termina un turno
        TurnInProcess = false;
    }

    // al hacerle clic se activa Fight y el argumento es el boton cliqueado que contiene al target
    public void Fight(FighterSelect targetButton)
    {
        ShowActionCanvas(false);
        //Debug.Log("Desactiva Canvas!!!!!");
        if (targetButton == null || targetButton.Fighter == null)// || targetButton.selfBbutton == null) 
        { Debug.Log("No se encuentra el botón del objetivo"); }
        // se especifica el target con la fucion EnemySelected

        Target = targetButton.Fighter;
        //Debug.Log("ATACANTE: " + ActiveFighter.Name);
        //Debug.Log("TARGET: " + Target.Name);

        CombatDescriptor.Clear();
        if (PlayerFighters.Contains(ActiveFighter)) { CombatDescriptor.AddTextLine(ActiveFighter.RealName + " usa " + AttackWeapon.Name); }
        else { CombatDescriptor.AddTextLine(ActiveFighter.Name + " usa " + AttackWeapon.Name); }
        //Muestra atacante y arma


        int damageToShow = -1;
        isCrit = false;
        SynergyDeterminant = 0;

        bool targetIsAlly = IsPlayerFighter(Target);
        bool attackerIsAlly = IsPlayerFighter(ActiveFighter);

        int missValue = Random.Range(0, 100); //La precisión del arma debe ser mayor que este valor para acertar el ataque.
        
        //ACIERTO
        if (AttackWeapon.BaseAccuracy > missValue)
        {
            #region SECUENCIA LÓGICA (11 pasos)
            // 1- Cambio de esperanza por sinergias
            // 2- Cálculo de efectividad (altera esperanza)
            // 3- Obtención factor esperanza
            // 4- Cálculo bonus elemental
            // 5- Cálculo de factor crítico
               #region 6- Cálculo de daño (5 subpasos)
                // 6.1- Cálculo de daño base
                // 6.2- Cálculo de multiplicador
                // 6.3- Cálculo de variabilidad
                // 6.4- Cálculo de daño final
                // 6.5- Corrección por crítico mortal
            #endregion
            // 7- Textos para Combat Descriptor (algunos)
            // 8- Aplicación de daño
            // 9- Alteración de esperanza por target derrotado, o aplicación de estados cuando el target sobrevive
            // 10- Alteración de esperanza por grupo en mal estado
            // 11- Activación de cooldown de arma atacante
            #endregion

            string critDesc, effectDesc, synerDesc; //Algunos posibles mensajes a mostrar
            critDesc = effectDesc = synerDesc = "";

            //PASO 1: SINERGIAS
            ApplySynergy(out synerDesc);

            //PASO 2: EFECTIVIDAD
            float effectivenessFact = CalculateEffectivenessFactor(out effectDesc);

            //PASO 3: FACTOR ESPERANZA
            float hopeFact = attackerIsAlly ? HopeManager.Instance.GetHopeFactor() : 1;

            //PASO 4: BONUS ELEMENTAL
            CombatType weaponType = AttackWeapon.TipoDeDañoQueAplica;
            float damageBonusFact = (ActiveFighter.TypeDamageBonuses[weaponType] - Target.TypeResistanceBonuses[weaponType]) * 0.25f;

            //PASO 5: CRÍTICO
            bool criticalAttack = false;
            float criticalFact = 0;
            int criticalRate = AttackWeapon.BaseCriticalRate + ActiveFighter.Luck; //Valor que se calcula como la suma de el índice de crítico del arma, y la suerte del atacante
            int criticalValue = Random.Range(0, 100);

            //El índice calculado debe superar el valor aleatorio para asestar crítico
            if (criticalRate > criticalValue)
            {
                criticalAttack = true;
                criticalFact = 1;
                critDesc = "¡¡¡GOLPE CRÍTICO!!!";
                isCrit = true;
                if (attackerIsAlly) 
                { 
                    string critHopeChange = HopeManager.Instance.ChangeHope(1, "Cambio por ataque crítico");
                    critDesc += " " + critHopeChange;
                }
            }
            else { isCrit = false; }

            //PASO 6: CÁLCULO DE DAÑO
            const int minDamage = 1;

            //PASO 6.1: DAÑO BASE
            //FÓRMULA DE DAÑO (Prototipo en uso. Debe ser bien definida más adelante)
            int baseDamage = Mathf.RoundToInt((AttackWeapon.BaseDamage * 0.1f) + ActiveFighter.Atack - Target.Defense * 0.8f);
            if (baseDamage < minDamage) { baseDamage = minDamage; }
            Debug.Log($"Base: {AttackWeapon.BaseDamage}/10 + {ActiveFighter.Atack} - {Target.Defense}*0.8");

            //PASO 6.2: MULTIPLICADOR
            float damageMultiplier = hopeFact + effectivenessFact + damageBonusFact + criticalFact;
            Debug.Log($"Multiplier: {hopeFact} + {effectivenessFact} + {damageBonusFact} + {criticalFact} = {damageMultiplier}");

            //PASO 6.3: VARIABILIDAD
            const float variabilityRatio = 0.1f;
            float variabilityFact = Random.Range(1 - variabilityRatio, 1 + variabilityRatio);

            //PASO 6.4: DAÑO FINAL
            int finalDamage = Mathf.RoundToInt(baseDamage * damageMultiplier * variabilityFact);
            Debug.Log($"Daño final: {baseDamage} * {damageMultiplier} * {variabilityFact} = {finalDamage}");
            //finalDamage = (int)(baseDamage * hopeFact * effectivenessFact * criticalFact * variabilityFact);
            if (finalDamage < minDamage) { finalDamage = minDamage; }

            //PASO 6.5: CORRECCIÓN POR CRÍTICO MORTAL
            //Se impide que un ataque crítico mate a un aliado si este hubiera sobrevivido al ataque normal. El aliado quedará con 1 HP.
            if(targetIsAlly  &&  criticalAttack)
            {
                int finalDamageNoCrit = Mathf.RoundToInt(baseDamage * (damageMultiplier - criticalFact) * variabilityFact);
                if (finalDamageNoCrit < minDamage) { finalDamageNoCrit = minDamage; }

                if (Target.CurrentHP - finalDamage <= 0 && Target.CurrentHP - finalDamageNoCrit > 0)
                {
                    finalDamage = Target.CurrentHP - 1;
                }
            }

            damageToShow = finalDamage;

            //PASO 7: TEXTOS PARA COMBAT DESCRIPTOR

            //Crítico, efectividad y sinergia se muestran
            CombatDescriptor.AddTextLine(critDesc);
            CombatDescriptor.AddTextLine(effectDesc);
            CombatDescriptor.AddTextLine(synerDesc);

            //Indica por texto quién recibió cuánto daño
            if (PlayerFighters.Contains(Target)) { CombatDescriptor.AddTextLine(Target.RealName + " pierde " + finalDamage + " de Salud"); }
            else { CombatDescriptor.AddTextLine(Target.Name + " pierde " + finalDamage + " de Salud"); }

            if (false    &&    attackerIsAlly && finalDamage == minDamage) 
            {
                string minDamageHopeChange = HopeManager.Instance.ChangeHope(-2, "Cambio por daño mínimo");
                CombatDescriptor.AddTextLine("Que petético... " + minDamageHopeChange); //Mensaje para daño mínimo
            }

            //PASO 8: APLICACIÓN DEL DAÑO
            Target.CurrentHP -= finalDamage;

            Target.OnTakeDamage?.Invoke();

            //PASO 9: LUCHADOR DERROTADO / APLICACIÓN DE ESTADOS
            //El objetivo es DERROTADO
            if (Target.CurrentHP <= 0)
            {
                Target.CurrentHP = 0;
                RemoveCombatStates(Target);
                IconManager.UpdateStateIcons(AllCombatFighters);
                Target.IsDefending = false;

                string defeatDesc;

                if (PlayerFighters.Contains(Target)) { defeatDesc = "¡" + Target.RealName + " ha sido derrotado! "; }
                else { defeatDesc = "¡" + Target.Name + " has been defeated! "; }
                 
                string defeatHopeChange = "";

                if (targetIsAlly)
                {
                    AlivePlayerFighters.Remove(Target);
                    defeatHopeChange = HopeManager.Instance.ChangeHope((sbyte)(AlivePlayerFighters.Count - 5), "Cambio por aliado muerto");
                }
                else
                {
                    AliveEnemyFighters.Remove(Target);
                    EnemyToDestroy = Target;
                    //defeatHopeChange = HopeManager.Instance.ChangeHope((sbyte)(Target.PowerRating + 1), "Cambio por vencer enemigo de poder " + Target.PowerRating);
                }

                Target.transform.rotation = new Quaternion(0, 0, 90, 0);

                defeatDesc += defeatHopeChange;
                CombatDescriptor.AddTextLine(defeatDesc);
            }
            //El objetivo SOBREVIVE
            else
            {
                //Aplicar estados
                foreach (CombatState weaponState in AttackWeapon.ListaDeEstadosQueAplica)
                {
                    if (!Target.States.Contains(weaponState))
                    {
                        Target.States.Add(weaponState);

                        //Muestra por texto el estado adquirido
                        if (PlayerFighters.Contains(Target)) { CombatDescriptor.AddTextLine(Target.RealName + " está " + weaponState.Name); }
                        else { CombatDescriptor.AddTextLine(Target.Name + " está " + weaponState.Name); }
                    }
                }
                IconManager.UpdateStateIcons(AllCombatFighters);
            }

            //PASO 10: COMPROBACIÓN DE HP GRUPAL
            //EL fin de estos métodos no es otro que cambiar la esperanza.
            CheckPartyHP();
            CheckHordeHP();

            //PASO 11: COOLDOWN DE ARMA
            ActiveFighter.WeaponCooldowns[AttackWeaponIndex] = AttackWeapon.BaseCooldown + 1;
        }
        //FALLO
        else
        {
            string failDesc = "¡Pero falló!";

            if(attackerIsAlly) 
            {
                string failHopeChange = HopeManager.Instance.ChangeHope(-1, "Cambio por ataque fallido");
                failDesc += " " + failHopeChange;
            }

            CombatDescriptor.AddTextLine(failDesc); //Indica ataque fallido
        }

        //Debug.Log("Precision: " + AttackWeapon.BaseAccuracy + " | damageToShow: " + damageToShow);

        // el botón imprime el daño infligido
        targetButton.ShowText(true, damageToShow, isCrit, SynergyDeterminant);
    }

    public void ApplySynergy(out string synerDesc)
    {
        #region SECUENCIA LÓGICA (3 pasos)
        // 1- Conteo de sinergias menos antisinergias
        // 2- Remoción de los estados del objetivo involucrados
        // 3- Para atacante aliado, cambio en la esperanza basado en el conteo de sinergias
        #endregion

        synerDesc = "";

        //Aplicar sinergias y antisinergias
        sbyte synergyCounter = 0;
        CombatType weaponType = AttackWeapon.TipoDeDañoQueAplica;
        List<CombatState> statesToErase = new List<CombatState>();
        foreach (CombatState targetState in Target.States)
        {
            if (weaponType.Sinergias.Contains(targetState))
            {
                synergyCounter++;
                statesToErase.Add(targetState);
            }
            else if (weaponType.AntiSinergias.Contains(targetState))
            {
                synergyCounter--;
                statesToErase.Add(targetState);
            }
        }

        foreach (CombatState state in statesToErase)
        {
            Target.States.Remove(state);
        }

        if(IsPlayerFighter(ActiveFighter))
        {
            sbyte hopeChangeMagnitude = 0;

            if      (synergyCounter == 1)   { hopeChangeMagnitude = 3; SynergyDeterminant = 1; }
            else if (synergyCounter >= 2)   { hopeChangeMagnitude = 4; SynergyDeterminant = 1; }
            else if (synergyCounter == -1)  { hopeChangeMagnitude = -3; SynergyDeterminant = -1; }
            else if (synergyCounter <= -2)  { hopeChangeMagnitude = -4; SynergyDeterminant = -1; }

            //Cambia esperanza, y prepara un mensaje sobre la sinergia generada
            if (hopeChangeMagnitude != 0)
            {
                if(hopeChangeMagnitude > 0) { synerDesc = "¡Sinergia generada! "; }
                else { synerDesc = "¡Anti-sinergia generada... ";  }
                string hopeChange = HopeManager.Instance.ChangeHope(hopeChangeMagnitude, "Cambio por sinergia");
                synerDesc += hopeChange;
            }
        }
    }

    public float CalculateEffectivenessFactor(out string effectDesc)
    {
        const float resistanceFactor    = -0.25f;
        const float weaknessFactor      = 0.5f;

        bool attackerIsAlly = IsPlayerFighter(ActiveFighter);

        CombatType weaponType = AttackWeapon.TipoDeDañoQueAplica;
        CombatType targetType = Target.Type;
        if(targetType.Resistencias.Contains(weaponType)) 
        {
            effectDesc = "Tipo No-efectivo... ";
            string hopeChange = "";
            if (attackerIsAlly) { hopeChange = HopeManager.Instance.ChangeHope(-1, "Cambio por inefectividad"); }
            effectDesc += hopeChange;
            return resistanceFactor; 
        }
        if(targetType.Debilidades.Contains(weaponType)) 
        {
            effectDesc = "¡Tipo Efectivo! ";
            string hopeChange = "";
            if (attackerIsAlly) { hopeChange = HopeManager.Instance.ChangeHope(1, "Cambio por efectividad"); }
            effectDesc += hopeChange;
            return weaknessFactor; 
        }

        effectDesc = "";
        return 0;
    }

    //Al finalizar un ataque, se comprueba el porcentaje de HP de ambos grupos respecto a su correspondiente HP máximo de grupo.
    //Cuando el HP general de los aliados baja de la mitad, disminuye la esperanza en término medio. Luego, para volver a aplicar el debuff, el grupo debe
    //haberse recuperado al menos hasta el 75% del HP general.
    public void CheckPartyHP()
    {
        PartyCurrentHP = 0;
        foreach(Fighter ally in AlivePlayerFighters)
        {
            PartyCurrentHP += ally.CurrentHP;
        }
        //Debug.Log("HP de grupo: " + PartyCurrentHP + "/" + PartyMaxHP);

        if(PartyIsFine  &&  PartyCurrentHP < PartyMaxHP * 0.5)
        {
            string hopeChange = HopeManager.Instance.ChangeHope(-3, "Cambio por mal estado del grupo");
            CombatDescriptor.AddTextLine("El equipo no se encuentra bien... " + hopeChange);
            PartyIsFine = false;
        }
        else if(!PartyIsFine  &&  PartyCurrentHP >= PartyMaxHP * 0.75)
        {
            PartyIsFine = true;
        }
    }
    //Cuando el HP general de la horda enemiga baja de la mitad, aumenta la esperanza en término bajo. No está contemplado que los enemigos
    //recuperen vida, con lo que el buff se aplicaría como máximo una vez por combate.
    //El buff es menor que el debuff pensando, desde el lado jugable, que los combates ganados serán más que los combates que pondrán en aprietos al
    //jugador, y desde el lado realista, que el miedo a perderlo todo para siempre es más grande que el gozo de superar un obstáculo pequeño
    //repetitivamente.
    public void CheckHordeHP()
    {
        HordeCurrentHP = 0;
        foreach(Fighter enemy in AliveEnemyFighters)
        {
            HordeCurrentHP += enemy.CurrentHP;
        }
        //Debug.Log("HP de horda: " + HordeCurrentHP + "/" + HordeMaxHP);

        if(HordeIsFine  &&  HordeCurrentHP < HordeMaxHP * 0.5)
        {
            string hopeChange = HopeManager.Instance.ChangeHope(2, "Cambio por mal estado de la horda enemiga");

            string hordeHPDesc = "";
            if(HordeCurrentHP <= 0) { hordeHPDesc = "¡Cuánta potencia!"; }
            else if(EnemyFighters.Count == 1) { hordeHPDesc = "¡El enemigo se encuentra débil! "; }
            else { hordeHPDesc = "¡Los enemigos se encuentran débiles! "; }
            hordeHPDesc += hopeChange;
            CombatDescriptor.AddTextLine(hordeHPDesc);

            HordeIsFine = false;
        }
    }

    public void RemoveAllCombatStates()
    {
        bool thereAreStates = false;
        foreach(Fighter f in AllCombatFighters)
        {
            bool fighterHasStates = RemoveCombatStates(f);
            if(!thereAreStates) { thereAreStates = fighterHasStates; }
        }

        if(thereAreStates) 
        {
            CombatDescriptor.AddTextLine("Todos los estados se han disipado", 1.5f);
            IconManager.UpdateStateIcons(AllCombatFighters);
            Debug.Log("HAY ESTADOS");
        }
    }

    public bool RemoveCombatStates(Fighter fighter)
    {
        if(fighter.States.Count == 0) { return false; }
        fighter.States.Clear();
        return true;
    }

    public void ResetWeaponCooldowns()
    {
        foreach(Fighter fighter in PlayerFighters)
        {
            for(byte i = 0; i<4;i++)
            {
                fighter.WeaponCooldowns[i] = 0;
            }
            /*
            foreach(Weapon weapon in fighter.Weapons)
            {
                if(weapon != null)
                {
                    weapon.CurrentCooldown = 0;
                }
            }
            */
        }
    }

    public void DiminishWeaponCooldowns()
    {
        foreach (Fighter fighter in AllCombatFighters)
        {
            for(byte i = 0; i<4;i++)
            {
                if(fighter.WeaponCooldowns[i] > 0)
                {
                    fighter.WeaponCooldowns[i]--;
                }
            }
            /*
            foreach (Weapon weapon in fighter.Weapons)
            {
                if (weapon != null  &&  weapon.CurrentCooldown > 0)
                {
                    weapon.CurrentCooldown--;
                }
            }
            */
        }
    }

    public void ActionSelection()
    {
        // se vacía el panel completamente
        CleanPanelSelecion();

        // cuando no se ha seleccionado una accion
        // se crea un botón por cada una de las acciones disponibles
        if (!GameManager.Instance.ConfirmationClick)
        {
            GameObject actionButton = Instantiate(PrefabActionButton);

            actionButton.GetComponent<ActionButton>().initialActionText = "Atacar";
            actionButton.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(actionButton);

            GameObject consumibles = Instantiate(PrefabActionButton);

            consumibles.GetComponent<ActionButton>().initialActionText = "Consumible";
            consumibles.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(consumibles);

            GameObject defensa = Instantiate(PrefabActionButton);

            defensa.GetComponent<ActionButton>().initialActionText = "Defensa";
            defensa.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(defensa);

            GameObject huir = Instantiate(PrefabActionButton);

            huir.GetComponent<ActionButton>().initialActionText = "Huir";
            huir.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(huir);
        }

        // si una accion ya fue seleccionada
        // se crea un botón cancelar, que permite cancelar la accion actual
        else
        {
            GameObject cancel = Instantiate(PrefabActionButton);
            cancel.GetComponent<ActionButton>().initialActionText = "Cancelar";
            cancel.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(cancel);

        // luego, dependiendo de la acción escogida se crean sus respectivos botones
            #region Ataque -> Weapon Selection
            if (GameManager.Instance.OnAttack)
            {
                Action = null;
                AttackWeapon = null;

                for (int i = 0; i < 4; i++)
                {
                    float r;
                    float g;
                    float b;
                    int cantidad = 0;
                    Color buttonColor = Color.white;

                    var W = ActiveFighter.Weapons[i];
                    if (W != null)
                    {
                        GameObject weaponButton = Instantiate(PrefabWeaponButton);

                       // weaponButton.GetComponent<WeaponSpecs>().weaponDamage.text = "damage: " + W.BaseDamage.ToString() + " - type:" + W.TipoDeDañoQueAplica.Name.ToString();
                        weaponButton.GetComponent<WeaponSpecs>().weaponName.text = W.Name;
                        weaponButton.GetComponent<WeaponSpecs>().thisWeapon = W;
                        weaponButton.GetComponent<WeaponSpecs>().IndexOfFighterWeapon = i;

                        if (ActiveFighter.WeaponCooldowns[i] > 0) weaponButton.GetComponent<WeaponSpecs>().GetComponent<Button>().interactable = false;


                        foreach (Fighter f in AliveEnemyFighters)
                        {
                            foreach (CombatState targetState in f.States)
                            {
                                if (W.TipoDeDañoQueAplica.Sinergias.Contains(targetState))
                                {
                                    cantidad++;
                                }
                                if(W.TipoDeDañoQueAplica.AntiSinergias.Contains(targetState))
                                { 
                                    cantidad--; 
                                }
                            }
                        }

                        if(cantidad > 0) 
                        {
                            r = 0;
                            g = 200 / 255f;
                            b = 130 / 255f;

                            buttonColor = new Color(r, g, b, 1f);
                        }
                        else if (cantidad < 0)
                        {
                            r = 70f;
                            g = 95f;
                            b = 95f;

                            buttonColor = new Color(r, g, b, 1f);
                        }
                        if (cantidad != 0)
                        {
                            Debug.Log($"cantidad de singergias del boton {i + 1} : {cantidad}");
                            weaponButton.GetComponent<WeaponSpecs>().synergyCount.text = cantidad.ToString();
                        }

                        weaponButton.transform.SetParent(PanelForActions.transform, false);
                        AllButtonsInPanel.Add(weaponButton);
                    }
                }
            }
            #endregion

            #region Consumibles
            if (GameManager.Instance.OnConsumible)
            {   
                foreach (var item in GameManager.Instance.AllConsumibles)
                {
                    GameObject itemButton = Instantiate(PrefabConsumibleButton);

                    itemButton.GetComponent<Button_Consumible>().itemName.text = item.Name;
                    itemButton.GetComponent<Button_Consumible>().thisItem = item;

                    itemButton.transform.SetParent(PanelForActions.transform, false);

                    AllButtonsInPanel.Add(itemButton);
                }
            }
            #endregion

            #region Defense
            if (GameManager.Instance.OnDefense)
            {
                ShowActionCanvas(false);
                // aumentar temporalmente la defensa del jugador activo
                // podría usarse un array de bonuses, útil también para consumibles
                ActiveFighter.Defense += DefenseValue; //Se aumentará la defensa del luchador hasta que vuelva a ser su turno
                ActiveFighter.IsDefending = true;

                CombatDescriptor.Clear(); //Si se llega a crear un método para aplicar la defensa, esta línea debe ir ahí
                CombatDescriptor.AddTextLine(ActiveFighter.RealName + " está defendiendose");

                // terminar turno
                GameManager.Instance.ConfirmationClick = false;
                CleanPanelSelecion();
                ActionDone = true;
            }
            #endregion

            #region FleeCombat
            if (GameManager.Instance.OnFleeCombat)
            {
                if (canFlee)
                {
                    //Se declara intención de huida
                    FleeActionSelected = true;

                    ShowActionCanvas(false);
                    CleanPanelSelecion();
                    ActionDone = true;

                    string fleeDesc = "El equipo huye del combate... ";
                    string fleeHopeChange = HopeManager.Instance.ChangeHope(-4, "Cambio por huida");
                    fleeDesc += fleeHopeChange;
                    CombatDescriptor.Clear();
                    CombatDescriptor.AddTextLine(fleeDesc, 1.5f); //El descriptor indica que el grupo huye, y cómo esto perjudica la esperanza
                }
                else
                {
                    CombatDescriptor.Clear();
                    CombatDescriptor.AddTextLine("¡No puedes huir de este combate!", 1.5f);
                }
                
            }
            #endregion
        }
    }

    private void CleanPanelSelecion()
    {
        foreach (GameObject button in AllButtonsInPanel)
        {
            Destroy(button);
        }
        AllButtonsInPanel.Clear();
    }

    private bool IsPlayerFighter(Fighter f)
    {
        PlayerFighter pFighter;
        return f.gameObject.TryGetComponent<PlayerFighter>(out pFighter);
    }

    public void LiftPlayerFighter(Fighter pf)
    {
        if(IsPlayerFighter(pf))
        {
            AlivePlayerFighters.Add(pf);
            pf.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else { Debug.Log("No se puede añadir un enemigo a la lista de aliados vivos"); }
    }

    //Verifica antes de cada turno si hay un bando ganador
    private sbyte CheckCombatState()
    {
        //Primero debe comprobarse el grupo del jugador. En un caso hipotético en que ambos bandos sean derrotados a la vez, sigue siendo una
        //derrota para el jugador.

        //GRUPO ALIADO

        bool defeat = true;
        //Se busca un luchador aliado cuyo HP sea mayor a 0. No se eliminan los luchadores muertos de la lista, pues podría ser posible que revivan
        //en combate.
        foreach(Fighter ally in PlayerFighters)
        {
            if(ally.CurrentHP > 0)
            {
                defeat = false;
                break;
            }
        }
        if(defeat) { return -1; } //No se encontraron aliados vivos

        //GRUPO ENEMIGO

        bool victory = true;
        //No es convencional que un enemigo resucite en un RPG, por lo que los enemigos podrían eliminarse de la lista al morir, y así sería
        //suficiente con consultar Count para comprobar la victoria.
        foreach(Fighter enemy in EnemyFighters)
        {
            if(enemy.CurrentHP > 0)
            {
                victory = false;
                break;
            }
        }
        if(victory) { return 1; } //No se encontraron enemigos vivos

        return 0; //Ambos bandos persisten aún.
    }
}
