using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TMP_Dropdown CurrentFighterConsume;

    public TextMeshProUGUI Descriptor;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Health;

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
        Name.text = currentFighter.RealName;
        if (currentFighter.CurrentHP == 0)
        {
            Health.text = "<#3e3e3e>" + currentFighter.CurrentHP.ToString() + "</color>" + " / " + currentFighter.MaxHP.ToString();
        }
        else if (currentFighter.CurrentHP == currentFighter.MaxHP)
        {
            Health.text = "<#00FF00>" + currentFighter.CurrentHP.ToString() + "</color>" + " / " + currentFighter.MaxHP.ToString();
        }
        else
        {
            Health.text = "<#C0C0C0>" + currentFighter.CurrentHP.ToString() + "</color>" + " / " + currentFighter.MaxHP.ToString();
        }
        
        Image.sprite = currentFighter.GetComponentInChildren<SpriteRenderer>().sprite;
        Image.preserveAspect = true;

        if(SelectedConsumible != null) { Descriptor.text = SelectedConsumible.Description; }
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
