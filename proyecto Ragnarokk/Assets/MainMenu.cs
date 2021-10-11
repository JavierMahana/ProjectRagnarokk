using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject StartButton;
    public GameObject ControlsButton;
    public GameObject CreditsButton;
    public GameObject QuitButton;
    public GameObject ReturnButton;

    public GameObject PanelText;

    public Text ControlsText;
    public Text CreditsText;

    void Start()
    {
        ActivateButtons(true);
    }

    void Update()
    {
        
    }
    
    public void ActivateButtons(bool active)
    {
        StartButton.SetActive(active);
        ControlsButton.SetActive(active);
        CreditsButton.SetActive(active);
        QuitButton.SetActive(active);
        ReturnButton.SetActive(!active);
        PanelText.SetActive(!active);

    }

    public void ClickStart()
    {
        SceneChanger.Instance.LoadMenuScene();

    }

    public void ClickControls()
    {
        ActivateButtons(false);
        ControlsText.gameObject.SetActive(true);
        CreditsText.gameObject.SetActive(false);
    }

    public void ClickCredits()
    {
        ActivateButtons(false);
        CreditsText.gameObject.SetActive(true);
        ControlsText.gameObject.SetActive(false);
    }

    public void ClickQuit()
    {
        SceneChanger.Instance.Exit();
    }

    public void ClickReturn()
    {
        ActivateButtons(true);
        ControlsText.gameObject.SetActive(false);
        CreditsText.gameObject.SetActive(false);
    }
}
