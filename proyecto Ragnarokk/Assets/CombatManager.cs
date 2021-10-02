using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


//ahora elementos de display pueden estar aca pero en el futuro moverlos.
public class CombatManager : MonoBehaviour
{
   
    public GameObject CombatCanvas;

    public GameObject PlayerButtons;

    public GameObject CanvasForClickFighter;
    public GameObject FighterClickButton;



    private GameManager gameManager;

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



    bool initilaized;
    //esta es la base vacia de cualquier fighter.
    public GameObject FighterBasePrefab;

    //la idea es que aca este o posible combate en el juegouna lista de tod.   




    public void InitCombatScene(CombatEncounter encounter)
    {
        //ACA ABRIA Q COLOCAR LOS PERSONAJES EN SUS POCICIONES.
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        /*
        foreach (var pf in gameManager.PlayerFighters)
        {
            var playerButton = Instantiate(PlayerButtons, pf.transform.position, Quaternion.identity);
            playerButton.transform.SetParent(CombatCanvas.transform, false);

            playerButton.GetComponent<PlayerSelect>().PlayerAssigned = pf.GetComponent<Fighter>().gameObject;
            gameManager.PlayerButtons.Add(playerButton);
        }
        */

        gameManager.Enemies.Clear();
        gameManager.EnemyButtons.Clear();


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

        initilaized = true;
    }

        
    

    private void CreateEnemy(FighterData data, Vector3 position)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        var enemyGameObject = Instantiate(FighterBasePrefab, position, Quaternion.identity);
        gameManager.SetDataToFighterGO(enemyGameObject, data);


        /*
           RectTransform rectTransform = enemyButton.GetComponent<RectTransform>();
             enemyButton.transform.position = Camera.main.ScreenToWorldPoint(myRect.transform.position);

           double x = enemyButton.transform.position.x;
           x *= 0.01;
           double y = enemyButton.transform.position.y;
           y *= 0.01;

           enemyButton.transform.position = new Vector2((float)x,(float)y);
        */
        Debug.Log("1" + enemyGameObject.transform.position);
        var enemyButton = Instantiate(FighterClickButton);

        RectTransform rectTransform = enemyButton.GetComponent<RectTransform>();

        Debug.Log("2"+ enemyButton.transform.position);

        enemyButton.GetComponent<FighterSelect>().Fighter = enemyGameObject;
        enemyButton.transform.SetParent(CombatCanvas.transform, true);

        Debug.Log("3"+ enemyButton.transform.position);

        Vector2 viewportPoint = Camera.main.WorldToViewportPoint(position);

        rectTransform.anchorMin = viewportPoint;
        rectTransform.anchorMax = viewportPoint;
        
        rectTransform.anchoredPosition = Vector2.zero;

        gameManager.Enemies.Add(enemyGameObject.GetComponent<Fighter>().gameObject);
        gameManager.EnemyButtons.Add(enemyButton.GetComponent<FighterSelect>().gameObject);
    }


    public void FighterAction(Button currentButton)
    {
        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        // Cambiar condicion a si es que es el turno del jugador
        if (gameManager.PlayerFighters.Count == 3)
        {
            CombatCanvas.SetActive(true);
            CombatCanvas.GetComponent<Canvas>().enabled = true;

            foreach (GameObject button in gameManager.EnemyButtons) {button.GetComponent<FighterSelect>().selfBbutton.interactable = false;}
            
            // si el jugador hace clic al botón accion en su turno
            if (currentButton.GetComponent<ActionButton>().ButtonPressed)
            {
                foreach (GameObject button in gameManager.EnemyButtons)
                {
                    button.GetComponent<FighterSelect>().selfBbutton.interactable = true;
                    button.GetComponent<FighterSelect>().selfBbutton.gameObject.SetActive(true);
                }

                //jugador de turno se escoge y luego se utiliza para calcular el daño en el Prefab Fighter Select
                gameManager.PlayerOnTurn = gameManager.PlayerFighters[0].gameObject;
                Debug.Log("Jugador en Turno: " + gameManager.PlayerOnTurn.GetComponent<Fighter>().Name);
            }
        }
        else
        {
            CombatCanvas.SetActive(false);
        }

        
    }


    //call backs unity
    private void Awake()
    {
        initilaized = false;
        // no se muestra el canvas de acciones hasta el turno de un playable character (FighterAction)
        CombatCanvas.GetComponent<Canvas>().enabled = true;
    }
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager.currentEncounter == null)
        {
            Debug.LogError("No hay encuentro en el game manager.");
            return;
        }
        InitCombatScene(gameManager.currentEncounter);
    }

    private void Update()
    {
        if (initilaized != true)
            return;

        
    }

}
