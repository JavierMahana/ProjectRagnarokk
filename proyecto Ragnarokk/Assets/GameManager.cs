using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;


//aca el jugador elije sus peleadores y ahi el juego le deja:
//empezar un encuentro
//curar a sus personajes
//elegir nuevos combatientes
public class GameManager : MonoBehaviour
{
    //es donde se colocaqn las unidades ya creadas. 
    public float yPlayerCharacters;

    Scene currentScene;
    public bool InFightingScene;

    public GameObject canvasPrefab;
    public GameObject buttonPrefab;

    public GameObject fighterPrefab;


    private GameObject canvasObj;






    public List<FighterData> AllFighterDatas = new List<FighterData>();

    public List<CombatEncounter> AllEncounters = new List<CombatEncounter>();


    //DontDestroyOnLoad
    //objetos de los fighters del player.
    [HideInInspector]
    public List<PlayerFighter> PlayerFighters = new List<PlayerFighter>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        CreateListButtonsOfPlayerFighters();
        //var canvasObj = Instantiate(canvasPrefab);
        //for (int i = 0; i < 10; i++)
        //{
        //    Instantiate(buttonPrefab, canvasObj.transform);
        //}
        
    }
    private void Update()
    {
        var playerFighters = FindObjectsOfType<PlayerFighter>(false);
        

        for (int i = 0; i < playerFighters.Length && i < 3; i++)
        {
            var pos = Camera.main.ViewportToWorldPoint(new Vector3(Mathf.Min(0.9f, (float)i / 2 + 0.1f), yPlayerCharacters, Camera.main.nearClipPlane));
            playerFighters[i].transform.position = pos;  
        }

        //Debug.Log($"{PlayerFighters.Count}");
        if (PlayerFighters.Count >= 3)
        {
            canvasObj.SetActive(false);

            //ACTIVA LA UI DE LOS ENFRENTAMIENTOS.
        }
        else
        {
            canvasObj.SetActive(true);
        }

    }

    public void CreateListButtonsOfPlayerFighters()
    {
        canvasObj = Instantiate(canvasPrefab);

        foreach (var figtherData in AllFighterDatas)
        {
            var buttonObj = Instantiate(buttonPrefab, canvasObj.transform);

            var buttonCode = buttonObj.GetComponent<Button_AddPlayerFighter>();
            buttonCode.data = figtherData;
            buttonCode.playerObjetcPrefab = fighterPrefab;
            buttonCode.gmManager = this;

            //var buttonUI = buttonObj.GetComponent<Button>();

            var text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = figtherData.Name;
        }
    }



    public void SetDataToFighterGO(GameObject fighterGO, FighterData data, bool playerFighter = false)
    {

        SpriteRenderer renderer;
        Fighter fighterComp;

        if (fighterGO.TryGetComponent(out renderer) && fighterGO.TryGetComponent(out fighterComp))
        {
            renderer.sprite = data.Sprite;

            fighterComp.Name = data.Name;
            fighterComp.Atack = data.Atack;
            fighterComp.Defense = data.Defense;
            fighterComp.Speed = data.Speed;
            fighterComp.MaxHP = data.MaxHP;

            for (int i = 0; i < fighterComp.Weapons.Length; i++)
            {
                fighterComp.Weapons[i] = data.DefaultWeapons[i];
            }
        }
        else
        {
            Debug.LogError("the fighter prefab needs: SpriteRenderer, Fighter,  Components");
            return;
        }

        //si no tienen el componente PlayerFighter se le agrega.
        if (playerFighter)
        {
            PlayerFighter pFighter;
            if (!fighterGO.TryGetComponent(out pFighter))
            {
                fighterGO.AddComponent<PlayerFighter>();
            }

            //fighterGO.GetComponent<PlayerFighter>();
            PlayerFighters.Add(fighterGO.GetComponent<PlayerFighter>());


        }
    }

    //public void CreateFighter(int i)
    //{
    //    //if()
    //}
}
