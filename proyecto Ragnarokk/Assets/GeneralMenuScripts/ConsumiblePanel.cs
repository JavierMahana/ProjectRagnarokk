using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumiblePanel : MonoBehaviour
{
    public Text Description;
    public Consumible SelectedConsumible;
    public List<GameObject> AllButtonsInPanel = new List<GameObject>();
    public GameObject PrefabConsumibleButton;

    public Fighter currentFighter;

    public Dropdown CurrentFighterConsume;

    public Text Name;
    public Text Health;
    public Image Image;

    public string Panelname = "Consumables";

    private void Start()
    {
        ClearPanel();
        FillPanel();
    }

    void Update()
    {
        // actualmente el texto de las opciones es 1,2,3
        // al implementar el nombre de cada personaje, su nombre ocupara sus lugares.

        // aqui cambiar el texto 1,2,3 por los respectivos nobmre de cada fighter
        //////////////////////////////////////////////////////////////////////////

        // revisa la opcion actual del dropdown
        int menuIndex = CurrentFighterConsume.value;
        string currentText = CurrentFighterConsume.options[menuIndex].text;


        // o bien if(CurrentFighterConsume.value == x)
        if ("1" == currentText)
        {
            currentFighter = GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>();
        }

        if ("2" == currentText)
        {
            currentFighter = GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>();
        }

        if ("3" == currentText)
        {
            currentFighter = GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>();
        }
    }

    public void OnUse()
    {
        //apply current item into the player.
        SelectedConsumible.OnUse(currentFighter);
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        Name.text = currentFighter.Name;
        Health.text = currentFighter.CurrentHP.ToString() + " / " + currentFighter.MaxHP.ToString();
        Image.sprite = currentFighter.GetComponent<SpriteRenderer>().sprite;
        ClearPanel();
        FillPanel();
    }

    public void FillPanel()
    {
        //Llena con consumibles el panel
        foreach (var item in GameManager.Instance.AllConsumibles)
        {
            GameObject itemButton = Instantiate(PrefabConsumibleButton);

            itemButton.GetComponent<Button_Consumible>().itemName.text = item.Name;
            itemButton.GetComponent<Button_Consumible>().itemDescription.text = item.Description;
            itemButton.GetComponent<Button_Consumible>().thisItem = item;

            itemButton.transform.SetParent(transform, false);

            AllButtonsInPanel.Add(itemButton);
        }
    }

    public void ClearPanel()
    {
        AllButtonsInPanel.Clear();
    }
}
