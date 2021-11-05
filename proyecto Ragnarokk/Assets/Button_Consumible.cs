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
        CombatManager combat = null;
        ConsumiblePanel panel = null;
       
        if(GameManager.Instance.GameState == GAME_STATE.COMBAT)
        {
            combat = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        }

        else if (GameManager.Instance.GameState == GAME_STATE.MENU)
        {
            panel = GameObject.Find("Consumibles").GetComponent<ConsumiblePanel>();
        }


        if (combat != null)
        {
            
            if (!ButtonPressed)
            {
                GameManager.Instance.ConfirmationClick = true;
                combat.SelectedConsumible = thisItem;
                foreach (GameObject button in combat.AllButtonsInPanel)
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
                combat.SelectedConsumible = null;
                ButtonPressed = false;
            }
            
        }
        else if (panel == null)
        {
            
            if (!ButtonPressed)
            {
                GameManager.Instance.ConfirmationClick = true;
                panel.SelectedConsumible = thisItem;
                foreach (GameObject button in panel.AllButtonsInPanel)
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
                panel.SelectedConsumible = null;
                ButtonPressed = false;
            }
            
        }
       
    }
        

}
