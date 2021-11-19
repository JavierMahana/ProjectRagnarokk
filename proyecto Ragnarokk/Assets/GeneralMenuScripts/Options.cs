using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject GameplayPanel;
    public Button gameplayButton;

    public GameObject GraphicsPanel;
    public GameObject AudioPanel;

    // 1 - 100 
    public Slider AudioGeneral; // (default 90)
    public Slider AudioSfx; // (default 80)
    public Slider AudioMusic; // (default 60)
    public Slider AudioAmbient; // (default 70)

    // 1- 10 (default 5)
    public Slider CombatSpeed;

    // in options
    public TMP_Dropdown Resolution;

    // 1: fullScreen || 2:windowed (default 1)
    public TMP_Dropdown Mode;

    public void OnOptions()
    {
        if (!GameplayPanel.activeSelf) { GameplayPanel.SetActive(true);}
    }

    private void Start()
    {
        UpdateOptionsPanels();
    }
    public void UpdateOptionsPanels()
    {
        AudioGeneral.value = PlayerPrefs.GetFloat("audioGeneral");
        AudioSfx.value = PlayerPrefs.GetFloat("audioSFX");
        AudioMusic.value = PlayerPrefs.GetFloat("audioMusic");
        AudioAmbient.value = PlayerPrefs.GetFloat("audioAmbient");

        CombatSpeed.value = PlayerPrefs.GetFloat("combatDescriptorSpeed");

        Mode.value = PlayerPrefs.GetInt("Fullscreen");
        if (Resolution.gameObject.activeSelf)
        {
            var width = PlayerPrefs.GetFloat("width", Screen.currentResolution.width);
            var height = PlayerPrefs.GetFloat("height", Screen.currentResolution.height);
            string resolution = width.ToString() + "X" + height.ToString();

            var size = Resolution.options.Count;
            for(int i = 0; i < size; i++)
            {
                if(Resolution.options[i].text == resolution)
                {
                    Resolution.value = i;
                }
            }
        }
    }

    public void SetDefault()
    {
        if(GameplayPanel.activeSelf)
        {
            CombatSpeed.value = 5;
        }

        if (GraphicsPanel.activeSelf)
        {
            Mode.value = 0;
        }

        if (AudioPanel.activeSelf)
        {
            AudioGeneral.value = 90;
            AudioSfx.value = 80;
            AudioMusic.value = 70;
            AudioAmbient.value = 60;
        }

        ApplyChanges();
    }

    public void ApplyChanges()
    {
        if (GameplayPanel.activeSelf)
        {
            // el default es 5, entonces el factor es 1
            // max es 2, min es 0.2
            PlayerPrefs.SetFloat("combatDescriptorSpeed", CombatSpeed.value / 5);
        }

        if (GraphicsPanel.activeSelf)
        {
            // pantalla completa
            PlayerPrefs.SetInt("Fullscreen", Mode.value);

            // windowed
            if (Mode.value == 1)
            {
                string line = Resolution.captionText.text;
                var ResolutionValues = line.Split('x');

                int width = int.Parse(ResolutionValues[0]);
                int height = int.Parse(ResolutionValues[1]);

                Debug.Log($"W = {width}  H = {height}");

                PlayerPrefs.SetInt("width", width);
                PlayerPrefs.SetInt("height", height);

            }    
        }

        if (AudioPanel.activeSelf)
        {
            PlayerPrefs.SetFloat("audioGeneral", AudioGeneral.value);
            PlayerPrefs.SetFloat("audioSFX", AudioSfx.value);
            PlayerPrefs.SetFloat("audioMusic", AudioMusic.value);
            PlayerPrefs.SetFloat("audioAmbient", AudioAmbient.value);
        }

        GameManager.Instance.SetPlayerConfiguration();
        FindObjectOfType<AudioManager>().Play("WeaponExchange");

    }
    void Update()
    {
        // si se escogio el modo windowed se puede usar el dropdown resolution
        Resolution.interactable = (Mode.value == 1) ? true : false;


    }
}
