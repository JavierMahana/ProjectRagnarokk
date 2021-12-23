using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GeneralMenu : MonoBehaviour
{
    public Button TeamButton;
    public Button WeaponButton;
    public Button ConsumibleButton;
    public Button OptionButton;
    public Button GlosaryButton;
    public Button SaveQuitButton;
    

    private ExplorationManager explorationManager;
    // sirve para el display del piso Actual
    public GameObject TitlePanel;
    public TextMeshProUGUI MenuTitle;
    public TMP_Dropdown MenuDropdown;

    public GameObject Background;

    //fighters
    public List<Fighter> TeamFighters = new List<Fighter>();

    //TeamCanvas
    public GameObject TeamCanvas;
    public GameObject Fighter1Panel;
    public GameObject Fighter2Panel;
    public GameObject Fighter3Panel;

    //Weapons
    public GameObject Weapons;

    //Consumibles
    public GameObject Consumibles;

    //Options
    public GameObject Options;

    //Glosary
    public GameObject Glosary;

    //Quit & Save
    public GameObject SaveQuit;
    public TextMeshProUGUI SnQtext;

    public GameObject ReturnButton;

    public List<GameObject> Panels = new List<GameObject>();

    public bool initialized;

    private string floor;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        initialized = true;
        floor = "Explorando Piso " + PlayerPrefs.GetInt("currentFloor");
        explorationManager = FindObjectOfType<ExplorationManager>();

        Panels.Clear();
        Panels.Add(TeamCanvas);
        Panels.Add(Weapons);
        Panels.Add(Consumibles);
        Panels.Add(Options);
        Panels.Add(SaveQuit);
        Panels.Add(Glosary);

        MenuDropdown.value = 0;
        OnClick();
    }
    public void Update()
    {
        #region RevisarQueBotones se pueden usar
        if(GameManager.Instance.GameState == GAME_STATE.PREGAME)
        {
            TeamButton.interactable = false;
            ConsumibleButton.interactable = false;
            WeaponButton.interactable = false;
        }

        if(GameManager.Instance.GameState == GAME_STATE.EXPLORATION)
        {
            TeamButton.interactable = true;
            ConsumibleButton.interactable = true;
            WeaponButton.interactable = true;
        }

        if(GameManager.Instance.GameState == GAME_STATE.COMBAT)
        {
            ConsumibleButton.interactable = false;
            WeaponButton.interactable = false;
        }

        #endregion


        var cm = FindObjectOfType<CombatManager>();

        // activacion del titulo del menu
        if(GameManager.Instance.GameState == GAME_STATE.PREGAME || GameManager.Instance.GameState == GAME_STATE.CREDITS || GameManager.Instance.GameState == GAME_STATE.SHOP || GameManager.Instance.GameState == GAME_STATE.TREASURE)
        {
            if(MenuDropdown.value == 0)
            {
                TitlePanel.gameObject.SetActive(false);
            }
        }
        else
        {
            TitlePanel.gameObject.SetActive(true);
        }

        // AL ESTAR en combate
        if (cm != null && GameManager.Instance.GameState == GAME_STATE.COMBAT && MenuTitle.text != "Ronda " + (cm.round + 1).ToString())
        {
           
            MenuTitle.text = "Ronda " + (cm.round+1).ToString();
            if(MenuDropdown.value == 0 && cm.AttackWeapon != null)
            {
                var weaponsButtons = FindObjectsOfType<WeaponSpecs>();
                foreach(WeaponSpecs ws in weaponsButtons)
                {
                    if(ws.thisWeapon == cm.AttackWeapon)
                    {
                        var fa = ws.GetComponent<ForAllButtons>();
                        if(!fa.IsButtonPressed)
                        {
                            fa.PressButton(fa.GetComponent<Button>());
                        }
                    }
                }
            }
        }
    }


    public void ChangeDropdownValue(int value)
    {
        if(MenuDropdown.value == value)
        {
            MenuDropdown.value = 0;
        }
        else
        {
            MenuDropdown.value = value;
        }
    }

    /// <param name="value">valor es un atributo pensado por si quieres sobreescribir el menu-dropdown</param>
    public void OnClick()
    {
        //revisa el dropdown actual al explorar
        if (MenuDropdown.value == 0)
        {
            ReturnButton.SetActive(false);

            HideAllPanels();
            explorationManager.gameObject.SetActive(true);
            Background.SetActive(false);

            //this.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

        }
        else
        {
            Debug.Log("No toy explorando.");
            ReturnButton.SetActive(true);
            Background.SetActive(true);
            explorationManager.gameObject.SetActive(false);
            //this.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.9f);
        }

        // resto de opciones cuando no se está explorando
        if (MenuDropdown.value == 1)
        {
            ActivatePanel(TeamCanvas, "Equipo");
            TeamCanvas.GetComponent<TeamCanvas>().SetUpPanel();
        }
        else if (MenuDropdown.value == 2)
        {
            ActivatePanel(Weapons, "Arsenal");
            Weapons.GetComponent<WeaponsPanel>().SetUpPanel();
        }
        else if (MenuDropdown.value == 3)
        {
            ActivatePanel(Consumibles, "Consumibles");
            Consumibles.GetComponent<ConsumiblePanel>().SetUpPanel();
        }
        else if (MenuDropdown.value == 4)
        {
            ActivatePanel(Options, "Opciones");
            Options.GetComponent<Options>().OnOptions();
        }
        else if (MenuDropdown.value == 5)
        {
            ActivatePanel(SaveQuit, "Salir y Guardar");
            if(GameManager.Instance.GameState == GAME_STATE.PREGAME) 
            {
                SnQtext.text = "¿Quieres salir del juego?";
            }
            if (GameManager.Instance.GameState == GAME_STATE.COMBAT)
            {
                SnQtext.text = "Al estar en combate no se peude guardar. ¿Quieres salir del juego?";
            }
                
        }
        else if (MenuDropdown.value == 6)
        {
            ActivatePanel(Glosary, "Glosario");
                
        }

        
            Debug.Log(MenuDropdown.options.Count);
        

            
            
        
             
    }
   
    // Activa el panel que se ingresa al método y desactiva el resto
    public void ActivatePanel(GameObject activate, string title)
    { 
        foreach (GameObject panel in Panels)
        {
            if(panel == activate)
            {
                if (!panel.activeSelf)
                {
                    panel.SetActive(true);
                }
            }
            else
            {
                if (panel.activeSelf)
                {
                    panel.SetActive(false);
                }
            }       
        }
        MenuTitle.text = title;
    }

    public void HideAllPanels()
    {
        foreach (GameObject panel in Panels)
        {
           if(panel.activeSelf)
            {
                panel.SetActive(false);
            }
        }

        //actualizar el piso a futuro
        MenuTitle.text = floor;
        if(GameManager.Instance.GameState == GAME_STATE.COMBAT)
        {
            var cm = FindObjectOfType<CombatManager>();
            if (cm != null) { MenuTitle.text = "round "+ cm.round.ToString(); }
                     
        }
    }


}
