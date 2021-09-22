using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


//ahora elementos de display pueden estar aca pero en el futuro moverlos.
public class CombatManager : MonoBehaviour
{
    private GameManager gameManager;


    [MinMaxSlider(minValue: 0, maxValue: 1)]
    float playerFightersX;

    [MinMaxSlider(minValue: 0, maxValue: 1)]
    float playerFightersMinY;
    [MinMaxSlider(minValue: 0, maxValue: 1)]
    float playerFightersMaxY;


    [MinMaxSlider(minValue: 0, maxValue: 1)]
    float enemyFightersX;

    [MinMaxSlider(minValue: 0, maxValue: 1)]
    float enemyFightersMinY;
    [MinMaxSlider(minValue: 0, maxValue: 1)]
    float enemyFightersMaxY;



    bool initilaized;
    //esta es la base vacia de cualquier fighter.
    public GameObject FighterBasePrefab;

    //la idea es que aca este o posible combate en el juegouna lista de tod.   




    public void InitCombatScene(GameManager gameManager, CombatEncounter encounter)
    {

        this.gameManager = gameManager;
        //ACA ABRIA Q COLOCAR LOS PERSONAJES EN SUS POCICIONES.


        int enemyCount = encounter.ListOfEncounterEnemies.Count;
        for (int i = 0; i < enemyCount; i++)
        {
            float t = ((enemyCount - 1) - i) / Mathf.Max(enemyCount - 1, 1);//t=1 cuando i = 0; t=0 cuando i = enemyCount-1
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
        var enemyGameObject = Instantiate(FighterBasePrefab, position, Quaternion.identity);

        gameManager.SetDataToFighterGO(enemyGameObject, data);
    }

    //call backs unity
    private void Awake()
    {
        initilaized = false;
        gameManager = GetComponent<GameManager>();
    }
    private void Update()
    {
        if (initilaized != true)
            return;

        
    }

}
