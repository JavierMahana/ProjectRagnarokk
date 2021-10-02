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

    private bool onWeaponSelect;

    public GameObject canvasPrefab;
    public GameObject playerFighterButtonPrefab;

    public GameObject canvasCombatPanel;

    private bool onFight = false;



    public GameObject healPlayerFightersButtonObj;
    public GameObject deletePlayerFightersButtonObj;


    //public List<GameObject> weaponSlotButtons;

    public GameObject selectWeaponView;

    public GameObject weaponSlotsCanvas;
    //public GameObject weaponsListButtonCanvasObj;

    public GameObject selectEncounterCanvasObj;
    private GameObject selectFighterCanvasObj;


    public void ShowView_SelectEncounter()
    {
        HideView_SelectCharacter();
        HideView_SelectWeapon();

        if (selectEncounterCanvasObj != null)
            selectEncounterCanvasObj.SetActive(true);

        if (healPlayerFightersButtonObj != null)
            healPlayerFightersButtonObj.SetActive(true);

        if (deletePlayerFightersButtonObj != null)
            deletePlayerFightersButtonObj.SetActive(true);

        if (weaponSlotsCanvas != null)
            weaponSlotsCanvas.SetActive(true);
    }
    private void HideView_SelectEncounter()
    {
        if (selectEncounterCanvasObj != null)
            selectEncounterCanvasObj.SetActive(false);

        if (healPlayerFightersButtonObj != null)
            healPlayerFightersButtonObj.SetActive(false);

        if (deletePlayerFightersButtonObj != null)
            deletePlayerFightersButtonObj.SetActive(false);

        if (weaponSlotsCanvas != null)
            weaponSlotsCanvas.SetActive(false);
    }

    public void ShowView_SelectCharacter()
    {
        HideView_SelectEncounter();
        HideView_SelectWeapon();

        if (selectFighterCanvasObj != null)
            selectFighterCanvasObj.SetActive(true);
    }
    private void HideView_SelectCharacter()
    {
        if (selectFighterCanvasObj != null)
            selectFighterCanvasObj.SetActive(false);
    }

    public void ShowView_SelectWeapon()
    {
        HideView_SelectCharacter();
        HideView_SelectEncounter();

        if (selectWeaponView != null)
            selectWeaponView.SetActive(true);
    }
    private void HideView_SelectWeapon()
    {
        if (selectWeaponView != null)
            selectWeaponView.SetActive(false);
    }

    public void ShowView_CombatSelection()
    {
        if(canvasCombatPanel != null)
        {
            canvasCombatPanel.SetActive(true);
        }
    }

    public void HideView_CombatSelection()
    {
        if (canvasCombatPanel != null)
        {
            canvasCombatPanel.SetActive(false);
        }
    }
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

        if (onWeaponSelect)
        {
            ShowView_SelectWeapon();
        }
        else if (GameManager.Instance.PlayerFighters.Count >= 3)
        {
            //HAY 3 COMBATIENTES.
            ShowView_SelectEncounter();
        }
        else
        {
            //HAY MENOS DE 3 COMBATIENTES.
            ShowView_SelectCharacter();
        }

        if (Input.GetKeyDown("space") && onFight) { onFight = false; }
        if (Input.GetKeyDown("space") && !onFight) { onFight = true; }
        
        if(onFight)
        {
            if(GameManager.Instance.PlayerFighters.Count == 3)
            {
                Debug.Log("Player Ready");
                if (GameManager.Instance.currentEncounter != null)
                {
                    Debug.Log("Encounter Ready");
                    ShowView_CombatSelection();
                }

            }
           
        }
        else
        {
            HideView_CombatSelection();
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


    public void StartWeaponSelect()
    {
        onWeaponSelect = true;
    }
    public void ReturnFromWeaponSelect()
    {
        onWeaponSelect = false;
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
