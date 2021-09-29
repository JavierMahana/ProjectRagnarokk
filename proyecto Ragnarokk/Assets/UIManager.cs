using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //es donde se colocaqn las unidades ya creadas. 
    public float xPlayerCharacters;
    public GameObject fighterPrefab;



    public GameObject canvasPrefab;
    public GameObject playerFighterButtonPrefab;

    



    public GameObject healPlayerFightersButtonObj;
    public GameObject deletePlayerFightersButtonObj;


    public List<GameObject> weaponSlotButtons;

    public GameObject weaponSlotsCanvas;
    public GameObject weaponsListButtonCanvasObj;

    public GameObject selectEncounterCanvasObj;
    private GameObject selectFighterCanvasObj;


    private void Awake()
    {
        
    }
    private void Start()
    {
        if (GameManager.Instance.InFightingScene)
        {
            //no crea la wea de personajes
        }
        else
        {

            //Se crea la UI de la escena de creacion
            CreateListButtonsOfPlayerFighters();
        }
    }
   
    private void Update()
    {

        for (int i = 0; i < GameManager.Instance.PlayerFighters.Count && i < 3; i++)
        {
            var pos = Camera.main.ViewportToWorldPoint(new Vector3(xPlayerCharacters, (float)(0.25 * (1 + i)), Camera.main.nearClipPlane));
            GameManager.Instance.PlayerFighters[i].transform.position = pos;
        }

        
        if (GameManager.Instance.PlayerFighters.Count >= 3)
        {
            if (selectFighterCanvasObj != null)
            {
                selectFighterCanvasObj.SetActive(false);
            }

            //ACTIVA LA UI DE LOS ENFRENTAMIENTOS.
            if (selectEncounterCanvasObj != null)
            {
                selectEncounterCanvasObj.SetActive(true);
            }

            if (healPlayerFightersButtonObj != null)
            {
                healPlayerFightersButtonObj.SetActive(true);
            }
            if (deletePlayerFightersButtonObj != null)
            {
                deletePlayerFightersButtonObj.SetActive(true);
            }
        }
        else
        {
            if (selectFighterCanvasObj != null)
            {
                selectFighterCanvasObj.SetActive(true);
            }


            if (selectEncounterCanvasObj != null)
            {
                selectEncounterCanvasObj.SetActive(false);
            }

            if (healPlayerFightersButtonObj != null)
            {
                healPlayerFightersButtonObj.SetActive(false);
            }

            if (deletePlayerFightersButtonObj != null)
            {
                deletePlayerFightersButtonObj.SetActive(false);
            }
        }
    }

    private void CreateListButtonsOfPlayerFighters()
    {
        selectFighterCanvasObj = Instantiate(canvasPrefab);
        var grid = selectFighterCanvasObj.AddComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(120,100);
        grid.childAlignment = TextAnchor.UpperCenter;


        foreach (var figtherData in GameManager.Instance.AllPlayerFighterDatas)
        {
            var buttonObj = Instantiate(playerFighterButtonPrefab, selectFighterCanvasObj.transform);

            var buttonCode = buttonObj.GetComponent<Button_AddPlayerFighter>();
            buttonCode.data = figtherData;
            buttonCode.playerObjetcPrefab = fighterPrefab;

            //var buttonUI = buttonObj.GetComponent<Button>();

            var text = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = figtherData.Name;
        }
    }

    public void DeletePlayerFighters()
    {
        var pfs = FindObjectsOfType<PlayerFighter>();
        foreach (var pf in pfs)
        {
            Destroy(pf.gameObject);
        }
    }

    public void HealPlayerFighters()
    {
        var pfs = FindObjectsOfType<PlayerFighter>();
        foreach (var pf in pfs)
        {
            var fighter = pf.GetComponent<Fighter>();
            fighter.CurrentHP = fighter.MaxHP;
        }
    }

}
