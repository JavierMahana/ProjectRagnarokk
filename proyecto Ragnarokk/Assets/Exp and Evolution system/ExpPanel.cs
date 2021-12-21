using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ExpPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject cantGiveExpPanel;
    public TextMeshProUGUI cantGiveEpxText;

    public TextMeshProUGUI Text;
    public Image Image;

    public ExpBar ExpBar;

    private ExperiencePanelManager manager;

    //esto es falso cuando hay un panel de no poder usar arriba.
    private bool canBeSelected;

    private int extraXPAmmount;
    private Fighter currFighter;


    private bool frameShown = false;
    private bool mouse_over = false;

    //esto es true cuando el manager diga q esta listo para seleccionar la exp extra.
    private bool canInteractuate = false;

    public void MarkAsReady()
    {
        canInteractuate = true;
    }
    private IEnumerator AddExtraXP()
    {
        yield return StartCoroutine(manager.ApplyExp(extraXPAmmount, currFighter, manager.ExpAplySpeed));

        //se aplica la experiencia extra. se carga la escena de exploracion.
        Debug.Log("Se termina de aplicar la XP");
        SceneChanger.Instance.LoadExplorationScene();
    }

    void Update()
    {
        if (mouse_over && Input.GetMouseButtonDown(0) && !frameShown && canBeSelected && canInteractuate)
        {
            StartCoroutine(AddExtraXP());
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
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
    }

    public void UpdatePanel()
    {
        Image.sprite = currFighter.Sprite;
        Text.text = currFighter.Name;
    }
    public void Show(Fighter currFighter, int extraXPAmmount, ExperiencePanelManager manager)
    {
        this.manager = manager;

        Image.sprite = currFighter.Sprite;
        Image.preserveAspect = true;
        Text.text = currFighter.Name;

        ExpBar.Init(currFighter);

        if (currFighter.CurrentHP <= 0)
        {
            cantGiveExpPanel.SetActive(true);
            cantGiveEpxText.text = "El aventurero está muerto";

            canBeSelected = false;
        }
        else if (currFighter.IsMaxLevel)
        {
            cantGiveExpPanel.SetActive(true);
            cantGiveEpxText.text = "El aventurero tiene nivel máximo";

            canBeSelected = false;
        }
        else
        {
            cantGiveExpPanel.SetActive(false);
            canBeSelected = true;
        }

        this.extraXPAmmount = extraXPAmmount;
        this.currFighter = currFighter;

        UpdatePanel();
    }
}
