using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //es donde se colocaqn las unidades ya creadas. 
    public float xPlayerCharacters;


    public GameObject canvasPrefab;
    public GameObject playerFighterButtonPrefab;
    public GameObject selectEncounterCanvasPrefab;


    public GameObject fighterPrefab;


    private GameObject selectFighterCanvasObj;
    private GameObject selectEncounterCanvasObj;



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
            selectEncounterCanvasObj = Instantiate(selectEncounterCanvasPrefab);
        }
    }


    private void Update()
    {
  
        for (int i = 0; i < GameManager.Instance.PlayerFighters.Count && i < 3; i++)
        {
            var pos = Camera.main.ViewportToWorldPoint(new Vector3(xPlayerCharacters, (float)(0.25 * (1+i)) , Camera.main.nearClipPlane));
            GameManager.Instance.PlayerFighters[i].transform.position = pos;
        }

        //Debug.Log($"{PlayerFighters.Count}");
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
        }

    }


    private void CreateListButtonsOfPlayerFighters()
    {
        selectFighterCanvasObj = Instantiate(canvasPrefab);

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



}
