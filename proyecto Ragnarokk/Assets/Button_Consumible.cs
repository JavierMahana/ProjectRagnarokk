using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_Consumible : MonoBehaviour
{
    public Text itemName;
    public Text itemDescription;
    public Consumible thisItem;

    public bool ButtonPressed;

    private void Start()
    {
        ButtonPressed = false;
    }

    public void OnClick()
    {
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        

        if (!ButtonPressed)
        {
            GameManager.Instance.ConfirmationClick = true;
            combatManager.SelectedConsumible = thisItem;
            foreach (GameObject button in combatManager.AllButtonsInPanel)
            {
                if (button.TryGetComponent(out Button_Consumible bc))
                {
                    bc.ButtonPressed = false;
                }
            }
            ButtonPressed = true;
        }
        else
        {
            combatManager.SelectedConsumible = null;
            ButtonPressed = false;
        }
    }

}
