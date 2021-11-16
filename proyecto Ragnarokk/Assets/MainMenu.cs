using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject PanelExplain;
    public GameObject PanelMenu;


    public GameObject StartButton;
    public GameObject ControlsButton;
    public GameObject Options;
    public GameObject CreditsButton;
    public GameObject QuitButton; 
    public GameObject ReturnButton;

    public GameObject PanelText;

    public TextMeshProUGUI ControlsText;
    public TextMeshProUGUI CreditsText;

    void Start()
    {
        if (PlayerPrefs.GetInt("firstTime") == 0)
        {
            PanelExplain.SetActive(true);
        }
        else
        {
            NormalMenuOn();
        }
    }
    
    public void NormalMenuOn()
    {
        PanelExplain.SetActive(false);
        PanelMenu.SetActive(true);
        ActivateButtons(true);
    }

    public void ActivateButtons(bool active)
    {
        StartButton.SetActive(active);
        ControlsButton.SetActive(active);
        CreditsButton.SetActive(active);
        QuitButton.SetActive(active);
        Options.SetActive(active);

        ReturnButton.SetActive(!active);
    }

    public void ClickStart()
    {
        SceneChanger.Instance.LoadMenuScene();

        //esto debería moverse a donde sea que termine el tutorial del juego.
        GameManager.Instance.TutorialComplete();
    }

    public void ClickQuit()
    {
        SceneChanger.Instance.Exit();
    }

}
