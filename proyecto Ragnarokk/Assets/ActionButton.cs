using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text actionText;
    public string initialActionText = "";
    public Color defaultColor;

    //indica que funcion cumple /defensa, ataque, consumible, etc
    public string thisCase;

    [HideInInspector]
    public bool ButtonPressed;

    // Update is called once per frame
    void Start()
    {
        ButtonPressed = false;
        //defaultColor = ColorBlock.defaultColorBlock.normalColor;
        actionText.text = initialActionText;
        thisCase = actionText.text;
    }

    public void PressedUpdate()
    {
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();

        if (!ButtonPressed)
        {
            combatManager.Action = initialActionText;
            actionText.text = "Cancel";
            ButtonPressed = true;

            // activa el ConfirmationClick que usa el else de ActionSelection
            GameManager.Instance.ConfirmationClick = true;

            switch (thisCase)
            {
                case "Attack":
                    Attack();
                    break;
                case "Consumable":
                    Consumible();
                    break;
                case "Defense":
                    Defense();
                    break;
                case "Cancel":
                    Cancel();
                    break;
                case "Flee Combat":
                    FleeCombat();
                    break;
                default:
                    Debug.Log("la accion actual no existe");
                    break;
            }

            // el else de ActionSelection da opciones dependiendo del thisCase del boton instanciado
            combatManager.ActionSelection();
        }
        else
        {
            actionText.text = initialActionText;

            //GetComponent<Image>().color = defaultColor;
            ButtonPressed = false;

            GameManager.Instance.ConfirmationClick = false;

            // al estar desactuvado el actionSelecion utiliza su if
            // mostrando todas las opciones del jugador
            combatManager.ActionSelection();
        }
    }

    public void Attack()
    {
        GameManager.Instance.OnAttack = true;
        GameManager.Instance.OnConsumible = false;
        GameManager.Instance.OnDefense = false;
        GameManager.Instance.OnFleeCombat = false;
    }
    public void Consumible()
    {
        GameManager.Instance.OnAttack = false;
        GameManager.Instance.OnConsumible = true;
        GameManager.Instance.OnDefense = false;
        GameManager.Instance.OnFleeCombat = false;
    }
    public void Defense()
    {
        GameManager.Instance.OnAttack = false;
        GameManager.Instance.OnConsumible = false;
        GameManager.Instance.OnDefense = true;
        GameManager.Instance.OnFleeCombat = false;
    }

    public void FleeCombat() 
    {
        GameManager.Instance.OnAttack = false;
        GameManager.Instance.OnConsumible = false;
        GameManager.Instance.OnDefense = false;
        GameManager.Instance.OnFleeCombat = true;
    }

    public void Cancel()
    {
        GameManager.Instance.OnAttack = false;
        GameManager.Instance.OnConsumible = false;
        GameManager.Instance.OnDefense = false;
        GameManager.Instance.OnFleeCombat = false;

        GameManager.Instance.ConfirmationClick = false;
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        combatManager.AttackWeapon = null;
        combatManager.SelectedConsumible = null;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string descripcion = "";
        var cm = FindObjectOfType<CombatManager>();
        switch (thisCase)
        {
            case "Attack":
                descripcion = "Pick from one of your four weapons slots and then click on an enemy.";
                break;
            case "Consumable":
                descripcion = "Pick from one of you consumibles and then select whoever you want to affect with it.";
                break;
            case "Defense":
                descripcion = "The fighter takes a defensive position, increasing it's defense until his next turn.";
                break;
            case "Cancel":
                descripcion = "Go back to the standard actions.";
                break;
            case "Flee Combat":
                descripcion = "The team leaves the current combat, without chance to continue and losing a big amount of hope.";
                break;
            default:
                Debug.Log("la accion actual no existe");
                break;
        }

        cm.SetlDescriptorText(descripcion);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // aún no se escogio el arma no se limpia el panel de informacion 
        if (!GameManager.Instance.ConfirmationClick) { FindObjectOfType<CombatManager>().ClearPanelDescriptor(); }
    }

}
