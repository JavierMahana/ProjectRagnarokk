using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public Text actionText;
    public string initialActionText;
    public Color initialColor = Color.white;

    [HideInInspector]
    public bool ButtonPressed;

    // Update is called once per frame
    void Start()
    {
        ButtonPressed = false;
        GetComponent<Image>().color = initialColor;
        actionText.text = initialActionText;
    }

    public void PressedUpdate()
    {
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();

        if (!ButtonPressed)
        {
            combatManager.Action = initialActionText;
            combatManager.AttackWeapon = combatManager.ActiveFighter.Weapons[0];

            GetComponent<Image>().color = Color.grey;
            actionText.text = "Cancel";
            ButtonPressed = true;
        }
        else
        {
            combatManager.Action = null;
            combatManager.AttackWeapon = null;

            GetComponent<Image>().color = initialColor;
            actionText.text = initialActionText;
            ButtonPressed = false;
        }
    }
}
