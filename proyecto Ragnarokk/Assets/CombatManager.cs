using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


//ahora elementos de display pueden estar aca pero en el futuro moverlos.
public class CombatManager : MonoBehaviour
{
    //eliminar las 2 lineas siguientes luego de testear los consumibles
    public Consumible item1;
    public Consumible item2;

    public GameObject CombatCanvas;

    public GameObject PrefabActionButton;
    public GameObject PrefabWeaponButton;
    public GameObject PrefabConsumibleButton;

    public GameObject PanelForActions;
    public GameObject FighterClickButton;
    List<GameObject> AllButtonsInPanel = new List<GameObject>();


    public GameObject PermanentCanvas;



    [Range(0.0f, 1.0f)]
    public float playerFightersX;

    [Range(0.0f, 1.0f)]
    public float playerFightersMinY;
    [Range(0.0f, 1.0f)]
    public float playerFightersMaxY;


    [Range(0.0f, 1.0f)]
    public float enemyFightersX;

    [Range(0.0f, 1.0f)]
    public float enemyFightersMinY;
    [Range(0.0f, 1.0f)]
    public float enemyFightersMaxY;


    [HideInInspector]
    public bool initialized;
    //esta es la base vacia de cualquier fighter.
    [HideInInspector]
    public GameObject FighterBasePrefab;

    //la idea es que aca este o posible combate en el juegouna lista de tod.   
    List<Fighter> PlayerFighters = new List<Fighter>();
    List<Fighter> EnemyFighters = new List<Fighter>();

    List<Fighter> AlivePlayerFighters = new List<Fighter>();

    [HideInInspector]
    public List<Fighter> AllCombatFighters = new List<Fighter>(); //Lista que almacenará a los luchadores del combate actual

    //public List<Fighter> AllAliveFighters = new List<Fighter>(); 
    [HideInInspector]
    public Fighter ActiveFighter;

    [HideInInspector]
    public FighterSelect CurrentPlayerButton;
    Vector2 OriginalButtonPos;

    sbyte ActiveFighterIndex = -1;

    [HideInInspector]
    public bool TurnInProcess;

    sbyte CombatState = 0;

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
    public Consumible SelectedConsumible = null;
    [HideInInspector]
    public Fighter Target = null;
    [HideInInspector]
    public bool Confirm = false; //Cuando sea true, la acción se debe llevar a cabo
    [HideInInspector]
    public bool Annulment = false; //Se usa cada vez que se quiere retroceder en la selección de algo

    [HideInInspector]
    //attack done podría cambiarse a ActionDone
    public bool AttackDone = false;

    //Comparador de rapidez para ordenar la lista de luchadores
    public int SpeedComparer(Fighter f1, Fighter f2)
    {
        int speed1 = f1.Speed;
        int speed2 = f2.Speed;

        return -speed1.CompareTo(speed2); //Signo menos para ordenar de mayor a menor
    }

    public void InitCombatScene(CombatEncounter encounter)
    {
        // Añade consumibles para testear, eliminar esto ya que luego los consumibles deben ser entregados
        // como recompensa de combate, exploración o comprados en la tienda.
        Consumible i1 = Instantiate(item1);
        Consumible i3 = Instantiate(item1);
        Consumible i2 = Instantiate(item2);
        GameManager.Instance.AllConsumibles.Add(i1);
        GameManager.Instance.AllConsumibles.Add(i2);
        GameManager.Instance.AllConsumibles.Add(i3);

        // limpia la lista de enemigos y sus botones más los botones aliados
        // antes de instanciar los del nuevo encuentro
        GameManager.Instance.Enemies.Clear();
        GameManager.Instance.EnemyButtons.Clear();
        GameManager.Instance.PlayerButtons.Clear();

        GameManager.Instance.ConfirmationClick = false;


        //Se obtienen los luchadores del jugador
        foreach (PlayerFighter pf in GameManager.Instance.PlayerFighters)
        {
            PlayerFighters.Add(pf.gameObject.GetComponent<Fighter>());
            Fighter fighter = PlayerFighters[PlayerFighters.Count - 1];
            string m = fighter.Name;
            foreach(CombatState state in fighter.States)
            {
                m += (" " + state.Name);
            }
            Debug.Log(m);

            #region Player Buttons
            var playerButton = Instantiate(FighterClickButton);

            RectTransform rectTransform = playerButton.GetComponent<RectTransform>();

            playerButton.GetComponent<FighterSelect>().Fighter = pf.GetComponent<Fighter>();
            playerButton.transform.SetParent(PermanentCanvas.transform, true);
            playerButton.GetComponent<Button>().interactable = false;

            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pf.transform.position);

            rectTransform.anchorMin = viewportPoint;
            rectTransform.anchorMax = viewportPoint;

            rectTransform.anchoredPosition = Vector2.zero;
            #endregion

            GameManager.Instance.PlayerButtons.Add(playerButton.GetComponent<FighterSelect>());
        }
        AlivePlayerFighters.AddRange(PlayerFighters);

        int enemyCount = encounter.ListOfEncounterEnemies.Count;
        for (int i = 0; i < enemyCount; i++)
        {

            float t = (((float)enemyCount - 1) - i) / Mathf.Max(enemyCount - 1, 1);//t=1 cuando i = 0; t=0 cuando i = enemyCount-1
            Debug.Log($"VALOR T: {t}");
            float viewPortY = Mathf.Lerp(enemyFightersMinY, enemyFightersMaxY, t);
            Vector3 viewPortPos = new Vector3(enemyFightersX, viewPortY, Camera.main.nearClipPlane);

            var worldPos = Camera.main.ViewportToWorldPoint(viewPortPos);

            var enemyData = encounter.ListOfEncounterEnemies[i];

            CreateEnemy(enemyData, worldPos);

        }

        //Se unen los luchadores en una lista
        AllCombatFighters.AddRange(PlayerFighters);
        AllCombatFighters.AddRange(EnemyFighters);

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
        var enemyGameObject = Instantiate(FighterBasePrefab, position, Quaternion.identity);
        GameManager.Instance.SetDataToFighterGO(enemyGameObject, data);


        var enemyButton = Instantiate(FighterClickButton);

        RectTransform rectTransform = enemyButton.GetComponent<RectTransform>();

        enemyButton.GetComponent<FighterSelect>().Fighter = enemyGameObject.GetComponent<Fighter>();
        enemyButton.transform.SetParent(CombatCanvas.transform, true);

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(position);

        rectTransform.anchorMin = viewportPoint;
        rectTransform.anchorMax = viewportPoint;
        
        rectTransform.anchoredPosition = Vector2.zero;

        GameManager.Instance.Enemies.Add(enemyGameObject.GetComponent<Fighter>());
        GameManager.Instance.EnemyButtons.Add(enemyButton.GetComponent<FighterSelect>());

        //Se llena una lista con los enemigos recién creados
        EnemyFighters.Add(enemyGameObject.GetComponent<Fighter>());
    }

    public void ShowFighterCanvas(bool show)
    {
        CombatCanvas.GetComponent<Canvas>().enabled = show;
        CombatCanvas.SetActive(show);
    }

    public void ShowEnemyInteractableButton(bool show)
    {
        foreach (FighterSelect button in GameManager.Instance.EnemyButtons)
        {
            button.selfBbutton.interactable = show;
        }
    }



    //call backs unity
    private void Awake()
    {
        initialized = false;
        TurnInProcess = false;
    }
    private void Start()
    {
        InitCombatScene(GameManager.Instance.currentEncounter);
        ShowFighterCanvas(false);
    }
    
    private void Update()
    {
        if (initialized != true)
            return;

        //Si no hay un turno en curso, se comprobará el estado del combate, e iniciará un nuevo turno, dentro de una corrutina.
        if(!TurnInProcess  &&  CombatState == 0)
        {
            CombatState = CheckCombatState();
            //El combate aún no termina, inicia el siguiente turno.
            if(CombatState == 0)
            {
                Debug.Log("Ejecutar turno");
                TurnInProcess = true;

                ShowFighterCanvas(false);
                CleanPanelSelecion();

                StartCoroutine(TurnAction());
            }
            else if(CombatState > 0)
            {
                RemoveAllCombatStates();

                var sceneChanger = FindObjectOfType<SceneChanger>();
                sceneChanger.ChangeScene("Victory");
                Debug.Log("VICTORIA");
            }
            else
            {
                var sceneChanger = FindObjectOfType<SceneChanger>();
                sceneChanger.ChangeScene("Defeat");
                Debug.Log("DERROTA");
            }
        }

        // constantemente se revisa si es que se va activar el botón para seleccionar enemigos
        ShowEnemyInteractableButton(GameManager.Instance.ConfirmationClick);

    }

    public void MoveActivePlayerButton(bool moveFoward)
    {
        if (moveFoward)
        {
            var position = (Vector2)ActiveFighter.transform.position;
            RectTransform rectTransform = CurrentPlayerButton.GetComponent<RectTransform>();
            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(position);

            rectTransform.anchorMin = viewportPoint;
            rectTransform.anchorMax = viewportPoint;

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

    private IEnumerator TurnAction()
    {
        //Se busca en la lista de luchadores al siguiente luchador vivo más rápido.
        do
        {
            ActiveFighterIndex++;
            if (ActiveFighterIndex >= AllCombatFighters.Count) 
            { 
                ActiveFighterIndex = 0; //No se sale de la lista
                RemoveAllCombatStates(); //Elimina los estados al finalizar un ciclo de turnos
            }
            ActiveFighter = AllCombatFighters[ActiveFighterIndex];
        } while (ActiveFighter.CurrentHP <= 0); //Comprueba que esté vivo.

        GameManager.Instance.PlayerOnTurn = ActiveFighter.gameObject; //¿PlayerOnTurn está en desuso?

        //Debug.Log(ActiveFighter.Name);


        var iniPos = ActiveFighter.transform.position;


        //TURNO DE UN ALIADO
        if (IsPlayerFighter(ActiveFighter))
        {
            //Debug.Log("Turno Aliado");
            AttackWeapon = null;
            SelectedConsumible = null;

            // activa canvas de acciones 
            ShowFighterCanvas(true);
            ActionSelection();
            

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
            yield return new WaitUntil(() => AttackDone);
            AttackDone = false;
            MoveActivePlayerButton(false);
        }
        //TURNO DE UN ENEMIGO
        else
        {
            //Debug.Log("Turno Enemigo");
            ShowFighterCanvas(false);

            ActiveFighter.transform.position += new Vector3((float)-2.5, 0, 0);

            AttackWeapon = ActiveFighter.Weapons[0];
            Target = AlivePlayerFighters[Random.Range(0, AlivePlayerFighters.Count)];

            // la variable button utiliza el boton de accion por default, sin embargo
            // debe ser reemplazado por el botón del jugador que será el target antes de llamar la funcion Fight
            // de no ser reemplazado en el siguiente while, es porque uno de los botones no fue asignado correctamente
            // a alguno de los 3 jugadores aliados
            FighterSelect targetButton = null;

            foreach(FighterSelect button in GameManager.Instance.PlayerButtons)
            {
                if (button.Fighter == Target)
                {
                    targetButton = button;
                    //Debug.Log("Boton Aliado encontrado por el enemigo");
                }
            }          
           
            Fight(targetButton);
            
            //Por seguridad se nulifican variables de ataque
            //(En el turno del jugador se anulan antes de la elección de cada una. ¿Sería preferible anularlas tras el ataque, igual que aquí?)
            AttackWeapon = null;
            Target = null;
        }

        yield return new WaitForSeconds(0.8f);

        ActiveFighter.transform.position = iniPos;
        // indica que termina un turno
        TurnInProcess = false;
    }

    /*
    private IEnumerator MakeDecision(object elementToDefine)
    {
        bool annulment = false;
        yield return new WaitUntil(() => (elementToDefine != null  ||  annulment));
    }
    */

    // al hacerle clic se activa Fight y el argumento es el boton cliqueado que contiene al target
    public void Fight(FighterSelect targetButton)
    {

        if(targetButton == null || targetButton.Fighter == null || targetButton.selfBbutton == null) { Debug.Log("No se encuentra el botón del objetivo"); }
        // se especifica el target con la fucion EnemySelected

        Target = targetButton.Fighter;
        //Debug.Log("ATACANTE: " + ActiveFighter.Name);
        //Debug.Log("OBJETIVO: " + Target.Name);

        Debug.Log("-------------------------------------------------------------------------------------");

        //Debug.Log(Target.Name + " es tipo " + Target.Type);
        string stateList = Target.Name;
        if (Target.States.Count != 0)
        {
            stateList += " tiene estados:";
            foreach (CombatState state in Target.States)
            {
                stateList += (" | " + state.Name);
            }
        }
        Debug.Log(stateList);

        float synergyFact = CalculateSynergyFactor();

        Debug.Log("Factor sinergia: " + synergyFact);

        //FÓRMULA DE DAÑO (Prototipo en uso. Debe ser bien definida más adelante)
        int damage = (AttackWeapon.BaseDamage / 25) + ActiveFighter.Atack - Target.Defense;
        if(damage < 0) { damage = 0;}
        Debug.Log("Daño inicial: " + damage);
        damage = (int)(damage * synergyFact);
        Debug.Log("Daño final: " + damage);

        //string e = IsPlayerFighter(ActiveFighter) ? "ALIADO " : "ENEMIGO ";
        //Debug.Log(e + ActiveFighter.Name + " ATACA con el ARMA " + AttackWeapon.Name + " al OBJETIVO " + Target.Name + " cuyo HP ERA " + Target.CurrentHP + " y AHORA ES " + (Target.CurrentHP - damage));
        Target.CurrentHP -= damage;
        Target.OnTakeDamage?.Invoke();

        if (Target.CurrentHP <= 0)
        {
            Target.CurrentHP = 0;
            RemoveCombatStates(Target);
            if(IsPlayerFighter(Target)) { AlivePlayerFighters.Remove(Target); }
            Target.transform.rotation = new Quaternion(0, 0, 90, 0);
        }
        else
        {
            //Aplicar estados
            foreach(CombatState weaponState in AttackWeapon.ListaDeEstadosQueAplica)
            {
                if(!Target.States.Contains(weaponState))
                {
                    Target.States.Add(weaponState);
                    Debug.Log("Aplicado: " + weaponState.Name);
                }
            }
        }

        // el botón imprime el daño infligido
        targetButton.ShowDamage(damage);
    }

    public float CalculateSynergyFactor()
    {
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
        float synergyFact = Mathf.Pow(2, synergyCounter);
        /*
        switch(synergyCounter)
        {
            case 1:     synergyFact = 2;        break;
            case >= 2:  synergyFact = 4;        break;
            case -1:    synergyFact = 0.5f;     break;
            case <= -2: synergyFact = 0.25f;    break;
            default:    synergyFact = 1;        break;
        }
        */

        foreach (CombatState state in statesToErase)
        {
            Target.States.Remove(state);
        }

        return synergyFact;
    }

    public void RemoveAllCombatStates()
    {
        foreach(Fighter f in AllCombatFighters)
        {
            RemoveCombatStates(f);
        }
    }

    public void RemoveCombatStates(Fighter fighter)
    {
        fighter.States.Clear();
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

            actionButton.GetComponent<ActionButton>().initialActionText = "Attack";
            actionButton.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(actionButton);

            GameObject consumibles = Instantiate(PrefabActionButton);

            consumibles.GetComponent<ActionButton>().initialActionText = "Consumable";
            consumibles.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(consumibles);

            GameObject defensa = Instantiate(PrefabActionButton);

            defensa.GetComponent<ActionButton>().initialActionText = "Defense";
            defensa.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(defensa);
        }

        // si una accion ya fue seleccionada
        // se crea un botón cancelar, que permite cancelar la accion actual
        else
        {
            GameObject cancel = Instantiate(PrefabActionButton);
            cancel.GetComponent<ActionButton>().initialActionText = "Cancel";
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
                    var W = ActiveFighter.Weapons[i];
                    if (W != null)
                    {
                        GameObject weaponButton = Instantiate(PrefabWeaponButton);

                        weaponButton.GetComponent<WeaponSpecs>().weaponDamage.text = "damage: " + W.BaseDamage.ToString() + " - type:" + W.TipoDeDañoQueAplica.Name.ToString();
                        weaponButton.GetComponent<WeaponSpecs>().weaponName.text = W.Name;
                        weaponButton.GetComponent<WeaponSpecs>().thisWeapon = W;

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

                    itemButton.GetComponent<Button_Consumible>().itemName.text = item.name;
                    itemButton.GetComponent<Button_Consumible>().itemDescription.text = item.description;
                    itemButton.GetComponent<Button_Consumible>().thisItem = item;

                    itemButton.transform.SetParent(PanelForActions.transform, false);

                    AllButtonsInPanel.Add(itemButton);
                }
            }
            #endregion

            #region Defense
            if (GameManager.Instance.OnDefense)
            {
                // aumentar temporalmente la defensa del jugador activo
                // podría usarse un array de bonuses, útil también para consumibles
                // ActiveFighter.Defense += 10;


                // terminar turno
                GameManager.Instance.ConfirmationClick = false;
                CleanPanelSelecion();
                AttackDone = true;
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
    }

    private bool IsPlayerFighter(Fighter f)
    {
        PlayerFighter pFighter;
        return f.gameObject.TryGetComponent<PlayerFighter>(out pFighter);
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
