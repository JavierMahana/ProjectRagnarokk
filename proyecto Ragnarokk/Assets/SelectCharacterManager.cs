using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectCharacterManager : MonoBehaviour
{
    public GameObject PanelExplain;
    public GameObject RestartButton;
    public GameObject StartButton;
    public GameObject SelectFighterButtons;
    public GameObject RandomTeam;
    
    [HideInInspector]
    public GameObject currentFighter;

    public GameObject SexCanvas;
    public GameObject NameCanvas;

    public TMP_InputField Inputfield;
    public TextMeshProUGUI FighterName;

    private bool isMale;
    private bool onNameChange;

    private void Start()
    {
        onNameChange = false;
        if (GameManager.Instance.CheckTutorialComplete())
        {
            PanelExplain.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(currentFighter != null && !onNameChange)
        {
            SexCanvas.SetActive(true);
            SelectFighterButtons.SetActive(false);
            RandomTeam.SetActive(false);
            var infoBox = FindObjectOfType<InfoBox>();
            if(infoBox != null) 
            {
                infoBox.GenericFormatObj.SetActive(true);
                infoBox.GenericDescriptionText.text = "Choose a Sex for your fighter. \n Your fighter's sex has no impact on game mechanics it is purely aesthetic."; 
            }
        }
        
        if(!onNameChange && !SexCanvas.activeSelf)
        {
            SelectFighterButtons.SetActive(true);
            RandomTeam.SetActive(true);
            StartButton.SetActive(false);

            if (GameManager.Instance.PlayerFighters.Count >= 3)
            {
                SelectFighterButtons.SetActive(false);
                RandomTeam.SetActive(false);
                StartButton.SetActive(true);
                
            }
        }

        if (GameManager.Instance.PlayerFighters.Count >= 1)
        {
            RestartButton.SetActive(true);
        }
        else
        {
            RestartButton.SetActive(false);
        }
    }

    public void FinishSetFighter()
    {
        foreach( PlayerFighter pf in GameManager.Instance.PlayerFighters)
        {
            if (pf == currentFighter.GetComponent<PlayerFighter>())
            {
                pf.GetComponent<Fighter>().isMale = isMale;
                pf.GetComponent<Fighter>().RealName = FighterName.text;

                var pfs = FindObjectsOfType<PlayerFighter>();
                int pfsCount = pfs.Length;
                pf.transform.position = new Vector2((-2 + pfsCount), 1);


                if(isMale) { pf.GetComponentInChildren<SpriteRenderer>().sprite = pf.GetComponent<Fighter>().SpriteMale; }
                else { pf.GetComponentInChildren<SpriteRenderer>().sprite = pf.GetComponent<Fighter>().SpriteFemale; }
            }
        }

        Inputfield.text = "";
        onNameChange = false;
        currentFighter = null;
        
        NameCanvas.SetActive(false);

    }

    public void SetSex(bool istrue)
    {
        isMale = istrue;

        SexCanvas.SetActive(false);
        onNameChange = true;

        var infoBox = FindObjectOfType<InfoBox>();
        if (infoBox != null)
        {
            infoBox.GenericFormatObj.SetActive(false);
        }
    }

    public void EmptyCurrentFighter()
    {
        currentFighter = null;

        var infoBox = FindObjectOfType<InfoBox>();
        if (infoBox != null)
        {
            infoBox.GenericFormatObj.SetActive(false);
        }
    }

    public void RandomTeamCreate()
    {
        GameManager.Instance.DeletePlayerFighters();
        currentFighter = null;
        var AddPlayerFighters = FindObjectsOfType<Button_AddPlayerFighter>();
        int sex;
        int cont;
        int numName;
        string randomName = "";

        if (AddPlayerFighters.Length != 0)
        {
            // 8 nombres
            string[] MaleNames = { "Sven", "Toby", "Rupert", "Kairos", "Semat", "Vulcan", "Matthew", "Galahad" };
            string[] FemaleNames = { "Jazmin", "Claudia", "Yerel", "Imoen", "Olga", "Vivianne", "Kelith", "Morgana" };
            
            for (int i = 0; i < 3; i++)
            {
                sex = Random.Range(0, 2);
                cont = Random.Range(0, 4);
                numName = Random.Range(0, 8);

                AddPlayerFighters[cont].CreatePlayerFighter();
                isMale = sex == 1 ? true : false;

                if (isMale)
                {
                    randomName = MaleNames[numName];
                }
                else
                {
                    randomName = FemaleNames[numName];
                }

                FighterName.text = randomName;
                FinishSetFighter();
            }
        }

       
    }

    public void OnNameChange(bool set)
    {
        onNameChange = set;
    }
}
