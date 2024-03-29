using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EvolutionPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI NameField;
    public Image ImageField;

    private FighterData currData;
    private Fighter evolvingFignter;

    private InfoBox infoBox;
    private ConfirmScreen confirmScreen;

    private bool frameShown = false;
    private bool mouse_over = false;

    private EvolutionPanelManager manager;

    private void Start()
    {
        confirmScreen = FindObjectOfType<ConfirmScreen>(true);
        if (confirmScreen == null)
            Debug.LogError("You need a confirm screen!");

        infoBox = FindObjectOfType<InfoBox>(true);
        if (infoBox == null)
            Debug.LogError("You need a info box!");
    }


    
    void Update()
    {

        if (mouse_over && Input.GetMouseButtonDown(0) && !frameShown && currData != null)
        {
           confirmScreen.Show(evolvingFignter, currData, manager);
        }
        frameShown = false;
    }

    private void OnEnable()
    {
        frameShown = true;
    }
    private void OnDisable()
    {
        mouse_over = false;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        if (currData != null)
        {
            infoBox.ShowInfo(currData);
        }
        else
        {
            infoBox.ShowInfo("Espacio Vac�o", "�Este espacio no tiene un arma, es seguro utilizarla!");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        if (manager != null && evolvingFignter != null)
        {
            infoBox.ShowInfo($"�{evolvingFignter.RealName} est� evolucionando!", $"Solo puedes escoger una evoluci�n. Escoge con cuidado...");
        }
        else
        {
            infoBox.Clear();
        }
    }


    public void Show(FighterData data, Fighter evolvingFighter, EvolutionPanelManager manager)
    {
        this.evolvingFignter = evolvingFighter;
        this.manager = manager;
        NameField.text = data.Name;
        ImageField.sprite = evolvingFighter.Sprite;
        ImageField.preserveAspect = true;
        currData = data;
    }


}
