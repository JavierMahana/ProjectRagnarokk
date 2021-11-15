using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GeneralMenu : MonoBehaviour
{
    private ExplorationManager explorationManager;
    public Text MenuTitle;

    public Dropdown MenuDropdown;

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

    //Quit & Save
    public GameObject SaveQuit;

    public GameObject ReturnButton;

    public List<GameObject> Panels = new List<GameObject>();

    public bool initialized;

    private void Awake()
    {
        //DontDestroyOnLoad(this);
        
    }
    void Start()
    {
        initialized = true;
        explorationManager = FindObjectOfType<ExplorationManager>();
     
        Panels.Clear();
        Panels.Add(TeamCanvas);
        Panels.Add(Weapons);
        Panels.Add(Consumibles);
        Panels.Add(Options);
        Panels.Add(SaveQuit);

        
        OnClick();
    }

    public void ChangeDropdownValue(int value)
    {
        MenuDropdown.value = value;
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
            this.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

        }
        else 
        {
            Debug.Log("No toy explorando.");

            ReturnButton.SetActive(true);
            Background.SetActive(true);
            explorationManager.gameObject.SetActive(false);
            this.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.9f);
        }


        // resto de opciones cuando no se está explorando
        if (MenuDropdown.value == 1) 
        { 
            ActivatePanel(TeamCanvas, "Team");
            TeamCanvas.GetComponent<TeamCanvas>().SetUpPanel();
        }
        if (MenuDropdown.value == 2)
        { 
            ActivatePanel(Weapons, "Weapons"); 
            Weapons.GetComponent<WeaponsPanel>().SetUpPanel(); 
        }
        if (MenuDropdown.value == 3) 
        { 
            ActivatePanel(Consumibles, "Consumables");
            Consumibles.GetComponent<ConsumiblePanel>().SetUpPanel();
        }
        if (MenuDropdown.value == 4) 
        {
            ActivatePanel(Options, "Options"); 
        }
        if (MenuDropdown.value == 5) 
        { 
            ActivatePanel(SaveQuit, "Save and Quit"); 
        }
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
        MenuTitle.text = "Exploring Floor" + " 1";
    }

}
