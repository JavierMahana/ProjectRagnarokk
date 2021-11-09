using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumiblePanel : MonoBehaviour
{
    public Consumible SelectedConsumible;
    public List<GameObject> AllButtonsInPanel = new List<GameObject>();

    public GameObject PrefabConsumibleButton;
    public GameObject ConsumablesPanel;
    public Button OnUseButton;

    public Fighter currentFighter;

    public Dropdown CurrentFighterConsume;

    public Text Name;
    public Text Health;
    public Image Image;

    public void SetUpPanel()
    {
        FillPanel();
    }

    void Update()
    {
        var GM = FindObjectOfType<GeneralMenu>();
        if (GM.MenuDropdown.value == 3)
        {
            if (CurrentFighterConsume.value == 0)
            {
                currentFighter = GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>();
            }

            if (CurrentFighterConsume.value == 1)
            {
                currentFighter = GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>();
            }

            if (CurrentFighterConsume.value == 2)
            {
                currentFighter = GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>();
            }
                
            OnUseButton.interactable = SelectedConsumible != null ? true : false;
                
            UpdatePanel();
        }
    }

    public void OnUse()
    {
        //apply current item into the player.
        if (SelectedConsumible != null)
        {
            Debug.Log(SelectedConsumible.Name);
            Debug.Log(currentFighter.Name);
            SelectedConsumible.OnUse(currentFighter);
        }
        else
        {
            Debug.Log("You must select an item from the Consumable's List");
        }
        
    }

    public void UpdatePanel()
    {
        Name.text = currentFighter.Name;
        Health.text = currentFighter.CurrentHP.ToString() + " / " + currentFighter.MaxHP.ToString();
        Image.sprite = currentFighter.GetComponent<SpriteRenderer>().sprite;
    }

    public void FillPanel()
    {
        ClearPanel();
        //Llena con consumibles el panel
        foreach (var item in GameManager.Instance.AllConsumibles)
        {
            GameObject itemButton = Instantiate(PrefabConsumibleButton);

            itemButton.GetComponent<Button_Consumible>().itemName.text = item.Name;
            itemButton.GetComponent<Button_Consumible>().thisItem = item;

            itemButton.transform.SetParent(ConsumablesPanel.transform, false);

            AllButtonsInPanel.Add(itemButton);
        }
    }

    public void ClearPanel()
    {
        foreach (GameObject button in AllButtonsInPanel)
        {
            Destroy(button);
        }
        AllButtonsInPanel.Clear();
    }
}
