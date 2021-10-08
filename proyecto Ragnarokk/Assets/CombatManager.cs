using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


//ahora elementos de display pueden estar aca pero en el futuro moverlos.
public class CombatManager : MonoBehaviour
{
   
    public GameObject CombatCanvas;

    public GameObject PrefabActionButton;
    public GameObject PrefabWeaponButton;

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



    bool initialized;
    //esta es la base vacia de cualquier fighter.
    public GameObject FighterBasePrefab;

    //la idea es que aca este o posible combate en el juegouna lista de tod.   

    List<Fighter> PlayerFighters = new List<Fighter>();
    List<Fighter> EnemyFighters = new List<Fighter>();

    List<Fighter> AlivePlayerFighters = new List<Fighter>();

    List<Fighter> AllCombatFighters = new List<Fighter>(); //Lista que almacenará a los luchadores del combate actual
    public Fighter ActiveFighter;

    sbyte ActiveFighterIndex = -1;

    public bool TurnInProcess;

    sbyte CombatState = 0;

    //VARIABLES DE ACCIÓN DE UN TURNO
    /// <summary>
    /// Se utilizan para las condiciones de continuación de la corrutina TurnAction, y para el cálculo de daño en el método Fight.
    /// Se pensaba hacerlas variables locales, pero el método que llama a la corrutina (Update) debe tener acceso a dichas variables para
    /// modificarlas, de otro modo, la corrutina no continuaría nunca.
    /// </summary>
    public string Action = null;
    //Variables de ataque
    public Weapon AttackWeapon = null;
    public Fighter Target = null;

    public bool Confirm = false; //Cuando sea true, la acción se debe llevar a cabo
    public bool Annulment = false; //Se usa cada vez que se quiere retroceder en la selección de algo

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

        //Se obtienen los luchadores del jugador
        foreach(PlayerFighter pf in GameManager.Instance.PlayerFighters)
        {
            PlayerFighters.Add(pf.gameObject.GetComponent<Fighter>());

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

            GameManager.Instance.PlayerButtons.Add(playerButton.GetComponent<FighterSelect>());

            #endregion

        }
        AlivePlayerFighters.AddRange(PlayerFighters);

        
        //ACA ABRIA Q COLOCAR LOS PERSONAJES EN SUS POCICIONES.
        /*
        foreach (var pf in gameManager.PlayerFighters)
        {
            var playerButton = Instantiate(PlayerButtons, pf.transform.position, Quaternion.identity);
            playerButton.transform.SetParent(CombatCanvas.transform, false);

            playerButton.GetComponent<PlayerSelect>().PlayerAssigned = pf.GetComponent<Fighter>().gameObject;
            gameManager.PlayerButtons.Add(playerButton);
        }
        */


        // limpia la lista de enemigos y sus botones
        // antes de instanciar los del nuevo encuentro
        GameManager.Instance.Enemies.Clear();
        GameManager.Instance.EnemyButtons.Clear();


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
        //ANALIZAR A FUTURO: No sé cómo funciona el método Sort por debajo, pero es posible que, al comparar dos valores iguales, no cambie el orden
        //de los elementos, provocando que los luchadores de igual velocidad tengan siempre el mismo orden de turnos. De hecho, los aliados atacan
        //antes que los enemigos de igual velocidad, probablemente porque los luchadores del jugador son añadidos a la lista primero.
        //Si se quisiera aleatorizar el orden de turnos para luchadores igual de rápidos, una solución podría ser modificar el método SpeedComparer
        //para que éste decida aleatoriamente el orden, lo cual se traduce en nunca retornar un 0.
        //Sin embargo, esta lista no está siendo modificada, por lo que el orden definido al iniciar un combate será permanente en cada ciclo de
        //turnos. Si se quisiera ir un paso más allá, podría reordenarse la lista cada vez que acabe un ciclo de turnos, permitiendo a todos los
        //luchadores de igual velocidad tener la oportunidad de actuar primero. Aunque no sé si llamar al método Sort repetidas veces sea óptimo.

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
            //Debug.Log("Actualizando");

            /*
            if(ActiveFighter != null)
            {
                Debug.Log(ActiveFighter.Name);
            }
            */

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
                CleanWeaponSelecion();

                StartCoroutine(TurnAction());
            }
            else if(CombatState > 0)
            {
                    Debug.Log("VICTORIA");
            }
            else
            {
                    Debug.Log("DERROTA");
            }
        }

        //SELECCIÓN DE ACCIONES
        //Hasta el momento no se puede elegir la acción realmente, solo se otorga una opción: atacar con el primer arma al primer oponente vivo.
        //Se usa la tecla Z para "elegir" una opción, y la tecla A para cancelar la última opción y volverla a "elegir".
        //SE DEBE REEMPLAZAR ESTE SISTEMA POR UNA INTERFAZ DE COMBATE APROPIADA.

        /*
        if(Action == null)
        {
            if(Input.GetKeyDown("z")) { Action = "fight"; }
        }
        else if(Action.Equals("fight"))
        {
            if(AttackWeapon == null)
            {
                if (Input.GetKeyDown("z")) { AttackWeapon = ActiveFighter.Weapons[0]; }
            }
            else if(Target == null)
            {
                sbyte i = -1;
                
                if (Input.GetKeyDown("z")) 
                {
                    Fighter target;
                    //Se busca el primer enemigo vivo
                    do
                    {
                        i++;
                        target = EnemyFighters[i];
                    } while (target.CurrentHP <= 0);
                    Target = target;
                }
            }
            else
            {
                Confirm = Input.GetKeyDown("z");
            }
        }
        Annulment = Input.GetKeyDown("a");
        */

        // constantemente se revisa si es que se va activar el botón para seleccionar enemigos
        ShowEnemyInteractableButton(GameManager.Instance.ConfirmationClick);

    }

    public void MovePanel()
    {
        var position = (Vector2)ActiveFighter.transform.position;
        RectTransform rectTransform = PanelForActions.GetComponent<RectTransform>();
        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(position);

        rectTransform.anchorMin = viewportPoint;
        rectTransform.anchorMax = viewportPoint;

        rectTransform.anchoredPosition = Vector2.zero;

        PanelForActions.GetComponent<RectTransform>().position = position;
    }
    private IEnumerator TurnAction()
    {
        //Se busca en la lista de luchadores al siguiente luchador vivo más rápido.
        do
        {
            ActiveFighterIndex++;
            if (ActiveFighterIndex >= AllCombatFighters.Count) { ActiveFighterIndex = 0; } //No se sale de la lista
            ActiveFighter = AllCombatFighters[ActiveFighterIndex];
        } while (ActiveFighter.CurrentHP <= 0); //Comprueba que esté vivo.

        GameManager.Instance.PlayerOnTurn = ActiveFighter.gameObject;

        //Debug.Log(ActiveFighter.Name);


        var iniPos = ActiveFighter.transform.position;


        //TURNO DE UN ALIADO
        if (IsPlayerFighter(ActiveFighter))
        {
            Debug.Log("Turno Aliado");
            ShowFighterCanvas(true);
            WeaponSelection();
            MovePanel();

            ActiveFighter.transform.position += new Vector3((float)2.5, 0, 0);
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

            yield return new WaitUntil(() => AttackDone);
            AttackDone = false;
        }
        //TURNO DE UN ENEMIGO
        else
        {
            Debug.Log("Turno Enemigo");
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
                    Debug.Log("Boton Aliado encontrado por el enemigo");
                }
            }          
           
            Fight(targetButton);
            
            /*
            Target.CurrentHP -= ActiveFighter.Atack - (int)(Target.Defense * 0.25);
                Debug.Log("HP " + Target.Name + ": " + Target.CurrentHP);
            if(Target.CurrentHP <= 0)
            {
                AlivePlayerFighters.Remove(Target);
                Target.gameObject.transform.rotation = new Quaternion(0, 0, 90, 0);
            }
            */
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
        Debug.Log("ATACANTE: " + ActiveFighter.Name);
        Debug.Log("OBJETIVO: " + Target.Name);
        //FÓRMULA DE DAÑO (Prototipo en desuso. Debe ser bien definida más adelante)
        int damage = (AttackWeapon.BaseDamage / 25) + ActiveFighter.Atack - Target.Defense;

        string e = IsPlayerFighter(ActiveFighter) ? "ALIADO " : "ENEMIGO ";
        Debug.Log(e + ActiveFighter.Name + " ATACA con el ARMA " + AttackWeapon.Name + " al OBJETIVO " + Target.Name + " cuyo HP ERA " + Target.CurrentHP + " y AHORA ES " + (Target.CurrentHP - damage));
        Target.CurrentHP -= damage;

        if (Target.CurrentHP <= 0)
        {
            Target.transform.rotation = new Quaternion(0, 0, 90, 0);
        }
        // el botón imprime el daño infligido
        targetButton.ShowDamage(damage);
        
    }

    public void WeaponSelection()
    {
        if(!GameManager.Instance.ConfirmationClick)
        {
            CleanWeaponSelecion();

            GameObject actionButton = Instantiate(PrefabActionButton);
            
            actionButton.GetComponent<ActionButton>().initialActionText = "Attack";
            actionButton.transform.SetParent(PanelForActions.transform, false);
            AllButtonsInPanel.Add(actionButton);
        }

        else
        {
            for(int i = 0; i < 4; i++)
            {
                var W = ActiveFighter.Weapons[i];
                if(W != null)
                {
                    GameObject actionButton = Instantiate(PrefabWeaponButton);

                    actionButton.GetComponent<WeaponSpecs>().weaponDamage.text = "damage" + W.BaseDamage.ToString();
                    actionButton.GetComponent<WeaponSpecs>().weaponName.text = W.Name;
                    actionButton.GetComponent<WeaponSpecs>().thisWeapon = W;

                    actionButton.transform.SetParent(PanelForActions.transform, false);

                    AllButtonsInPanel.Add(actionButton);
                }          
            }
        }
    }

    private void CleanWeaponSelecion()
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
