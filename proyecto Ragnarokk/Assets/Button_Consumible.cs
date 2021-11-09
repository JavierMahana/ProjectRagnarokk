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
    private Color defaultColor;


    private void Start()
    {
        ButtonPressed = false;
        defaultColor = ColorBlock.defaultColorBlock.normalColor;
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
            panel = FindObjectOfType<ConsumiblePanel>();
            if (panel != null)
            {
                Debug.Log("panel found");
            }
            else
            {
                Debug.Log("no panel found");
            }
            
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
        else if (panel != null)
        {
            if (!ButtonPressed)
            {
                foreach (GameObject button in panel.AllButtonsInPanel)
                {
                    if (button.TryGetComponent(out Button_Consumible bc))
                    {
                        bc.ButtonPressed = false;
                    }
                }

                Debug.Log("assign consumible");
                panel.SelectedConsumible = thisItem;
                ButtonPressed = true;
                Debug.Log(panel.SelectedConsumible.Name);
            }
            else
            {
                panel.SelectedConsumible = null;
                GetComponent<Image>().color = defaultColor;
                ButtonPressed = false;
            }
            
        }
       
    }
        

}
