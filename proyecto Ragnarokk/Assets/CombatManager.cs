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
    public List<GameObject> AllButtonsInPanel = new List<GameObject>();


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

    //LUCHADORES
    [HideInInspector]
    public List<Fighter> AllCombatFighters = new List<Fighter>(); //Lista que almacenará a los luchadores del combate actual

    //Luchadores aliados  
    List<Fighter> PlayerFighters = new List<Fighter>();
    List<Fighter> AlivePlayerFighters = new List<Fighter>();

    int PartyMaxHP;
    int PartyCurrentHP;
    bool PartyIsFine; //Se inicializa en true si el combate empieza con el HP general sobre el 75%.

    //Luchadores enemigos
    List<Fighter> EnemyFighters = new List<Fighter>();
    List<Fighter> AliveEnemyFighters = new List<Fighter>();

    int HordeMaxHP;
    int HordeCurrentHP;
    bool HordeIsFine;

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

            playerButton.GetComponent<FighterSelect>().Fighter = pf.GetComponent<Fighter>();
            playerButton.transform.SetParent(PermanentCanvas.transform, true);
            playerButton.GetComponent<Button>().interactable = false;

            Vector2 viewportPoint = Camera.main.WorldToViewportPoint(pf.transform.position);

            rectTransform.anchorMin = viewportPoint;
            rectTransform.anchorMax = viewportPoint;

            rectTransform.anchoredPosition = Vector2.zero;
            #endregion

            GameManager.Instance.PlayerButtons.Add(playerButton.GetComponent<FighterSelect>());

            j++;
        }
        PartyIsFine = (PartyCurrentHP >= PartyMaxHP * 0.75);
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
        HordeCurrentHP = HordeMaxHP;
        HordeIsFine = true;
        AliveEnemyFighters.AddRange(EnemyFighters);

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
        Fighter enemy = enemyGameObject.GetComponent<Fighter>();
        EnemyFighters.Add(enemy);
        HordeMaxHP += enemy.MaxHP;
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

        PartyMaxHP = 0;
        PartyCurrentHP = 0;
        PartyIsFine = false;

        HordeMaxHP = 0;
        HordeCurrentHP = 0;
        HordeIsFine = false;
    }
    private void Start()
    {
        InitCombatScene(GameManager.Instance.currentEncounter);
        ResetWeaponCooldowns();
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

                FindNextActiveFighter();
                StartCoroutine(TurnAction());
            }
            else if(CombatState > 0)
            {
                RemoveAllCombatStates();
                ResetWeaponCooldowns();
                HopeManager.Instance.ChangeHope(3, "Cambio por victoria");

                var sceneChanger = FindObjectOfType<SceneChanger>();
                //sceneChanger.ChangeScene("Victory");
                //Debug.Log("VICTORIA");
                sceneChanger.ChangeScene("Exploration");
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
        RemoveAllCombatStates();
        DiminishWeaponCooldowns();
    }

    private IEnumerator TurnAction()
    {

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
            yield return new WaitUntil(() => ActionDone);
            ActionDone = false;
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
        if (targetButton == null || targetButton.Fighter == null || targetButton.selfBbutton == null) { Debug.Log("No se encuentra el botón del objetivo"); }
        // se especifica el target con la fucion EnemySelected

        Target = targetButton.Fighter;
        //Debug.Log("ATACANTE: " + ActiveFighter.Name);
        //Debug.Log("OBJETIVO: " + Target.Name);

        int damageToShow = -1;

        bool targetIsAlly = IsPlayerFighter(Target);

        int missValue = Random.Range(0, 100); //La precisión del arma debe ser mayor que este valor para acertar el ataque.
        
        //ACIERTO
        if (AttackWeapon.BaseAccuracy > missValue)
        {
            #region SECUENCIA LÓGICA (9 pasos)
            // 1- Cambio de esperanza por sinergias
            // 2- Cálculo de efectividad (altera esperanza)
            // 3- Obtención factor esperanza
            // 4- Cálculo de daño base
            // 5- Cálculo de daño final (considera factores efectividad y esperanza) (incluye posterior disminución de esperanza por daño mínimo)
            // 6- Aplicación de daño
            // 7- Alteración de esperanza por target derrotado, o aplicación de estados cuando el target sobrevive
            // 8- Alteración de esperanza por grupo en mal estado
            // 9- Activación de cooldown de arma atacante
            #endregion

            Debug.Log("Precision " + AttackWeapon.BaseAccuracy + " > " + missValue);
            //PASO 1
            ApplySynergy();

            //PASO 2
            float effectivenessFact = CalculateEffectivenessFactor();

            //PASO 3
            float hopeFact = targetIsAlly ? 1 : HopeManager.Instance.GetHopeFactor();

            //Debug.Log("Factor efectividad: " + effectivenessFact);
            Debug.Log("Factor esperanza: " + hopeFact);

            const int minDamage = 1;

            //PASO 4
            //FÓRMULA DE DAÑO (Prototipo en uso. Debe ser bien definida más adelante)
            int damage = (AttackWeapon.BaseDamage / 25) + ActiveFighter.Atack - Target.Defense;

            if (damage < minDamage) { damage = minDamage; }

            //PASO 5
            //Debug.Log("Daño inicial: " + damage);
            damage = (int)(damage * hopeFact * effectivenessFact);
            if (damage < minDamage) { damage = minDamage; }
            damageToShow = damage;
            //Debug.Log("Daño final: " + damage);
            if (!targetIsAlly && damage == minDamage) { HopeManager.Instance.ChangeHope(-2, "Cambio por daño mínimo"); }

            //PASO 6
            //APLICACIÓN DEL DAÑO
            Target.CurrentHP -= damage;

            //string e = IsPlayerFighter(ActiveFighter) ? "ALIADO " : "ENEMIGO ";
            //Debug.Log(e + ActiveFighter.Name + " ATACA con el ARMA " + AttackWeapon.Name + " al OBJETIVO " + Target.Name + " cuyo HP ERA " + Target.CurrentHP + " y AHORA ES " + (Target.CurrentHP - damage));
            Debug.Log("Arma atacante: " + AttackWeapon.Name);

            Target.OnTakeDamage?.Invoke();

            //PASO 7
            //El objetivo es DERROTADO
            if (Target.CurrentHP <= 0)
            {
                Target.CurrentHP = 0;
                RemoveCombatStates(Target);
                if (targetIsAlly)
                {
                    AlivePlayerFighters.Remove(Target);
                    HopeManager.Instance.ChangeHope((sbyte)(AlivePlayerFighters.Count - 5), "Cambio por aliado muerto");
                }
                else
                {
                    AliveEnemyFighters.Remove(Target);
                    HopeManager.Instance.ChangeHope((sbyte)(Target.PowerRating + 1), "Cambio por vencer enemigo de poder " + Target.PowerRating);
                }
                Target.transform.rotation = new Quaternion(0, 0, 90, 0);
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
                        //Debug.Log("Aplicado: " + weaponState.Name);
                    }
                }
            }

            //PASO 8
            //EL fin de estos métodos no es otro que cambiar la esperanza.
            CheckPartyHP();
            CheckHordeHP();

            //PASO 9
            ActiveFighter.WeaponCooldowns[AttackWeaponIndex] = AttackWeapon.BaseCooldown + 1;
        }
        //FALLO
        else
        {
            if(!targetIsAlly) { HopeManager.Instance.ChangeHope(-2, "Cambio por ataque fallido"); }
            Debug.Log("Precision " + AttackWeapon.BaseAccuracy + " <= " + missValue);
        }

        //Debug.Log("Precision: " + AttackWeapon.BaseAccuracy + " | damageToShow: " + damageToShow);

        // el botón imprime el daño infligido
        targetButton.ShowDamage(damageToShow);
    }

    public void ApplySynergy()
    {
        #region SECUENCIA LÓGICA
        // 1- Conteo de sinergias menos antisinergias
        // 2- Remoción de los estados del objetivo involucrados
        // 3- Para atacante aliado, cambio en la esperanza basado en el conteo de sinergias
        #endregion

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
        //float synergyFact = Mathf.Pow(2, synergyCounter);

        foreach (CombatState state in statesToErase)
        {
            Target.States.Remove(state);
        }

        if(IsPlayerFighter(ActiveFighter))
        {
            sbyte hopeChangeMagnitude = 0;
            switch (synergyCounter)
            {
                case 1: hopeChangeMagnitude = 3; break;
                case 2: hopeChangeMagnitude = 4; break;
                case -1: hopeChangeMagnitude = -3; break;
                case -2: hopeChangeMagnitude = -4; break;
            }
            if (hopeChangeMagnitude != 0)
            {
                HopeManager.Instance.ChangeHope(hopeChangeMagnitude, "Cambio por sinergia");
            }
        }

        //return synergyFact;
    }

    public float CalculateEffectivenessFactor()
    {
        const float resistanceFactor    = 0.75f;
        const float weaknessFactor      = 1.5f;

        CombatType weaponType = AttackWeapon.TipoDeDañoQueAplica;
        CombatType targetType = Target.Type;
        if(targetType.Resistencias.Contains(weaponType)) 
        {
            HopeManager.Instance.ChangeHope(-2, "Cambio por inefectividad");
            return resistanceFactor; 
        }
        if(targetType.Debilidades.Contains(weaponType)) 
        {
            HopeManager.Instance.ChangeHope(2, "Cambio por efectividad");
            return weaknessFactor; 
        }

        return 1;
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
        Debug.Log("HP de grupo: " + PartyCurrentHP + "/" + PartyMaxHP);

        if(PartyIsFine  &&  PartyCurrentHP < PartyMaxHP * 0.5)
        {
            HopeManager.Instance.ChangeHope(-3, "Cambio por mal estado del grupo");
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
        Debug.Log("HP de horda: " + HordeCurrentHP + "/" + HordeMaxHP);

        if(HordeIsFine  &&  HordeCurrentHP < HordeMaxHP * 0.5)
        {
            HopeManager.Instance.ChangeHope(2, "Cambio por mal estado de la horda enemiga");
            HordeIsFine = false;
        }
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

            GameObject huida = Instantiate(PrefabActionButton);

            huida.GetComponent<ActionButton>().initialActionText = "Flee Combat";
            huida.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(huida);
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
                        weaponButton.GetComponent<WeaponSpecs>().IndexOfFighterWeapon = i;

                        if (ActiveFighter.WeaponCooldowns[i] > 0) weaponButton.GetComponent<WeaponSpecs>().GetComponent<Button>().interactable = false;
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
                // aumentar temporalmente la defensa del jugador activo
                // podría usarse un array de bonuses, útil también para consumibles
                // ActiveFighter.Defense += 10;


                // terminar turno
                GameManager.Instance.ConfirmationClick = false;
                CleanPanelSelecion();
                ActionDone = true;
            }
            #endregion

            #region FleeCombat
            if (GameManager.Instance.OnFleeCombat)
            {
                // se escapa del combate

                // terminar turno
                GameManager.Instance.ConfirmationClick = false;
                CleanPanelSelecion();
                ActionDone = true;
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
