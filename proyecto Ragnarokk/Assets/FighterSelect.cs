using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FighterSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //public Button selfBbutton;

    public Fighter Fighter;

    public TextMeshProUGUI showText;

    public int showTimer;

    private string defaultText = "";
    private Color normalColor;
    private Color healColor;
    private Color critColor;

    //agregar despues una lista de imagenes (estados) dentro de un canvas (atributo) que se activa si hay efectos en Fighter

    void Start()
    {
        float r, g, b;

        r = 1f;
        g = 120 / 255f;
        b = 0;
        healColor = new Color(r, g, b, 0.8f);

        r = 0;
        g = 200 / 255f;
        b = 130 / 255f;
        critColor = new Color(r, g, b, 1);


        normalColor = showText.color;
        healColor = Color.cyan;
        critColor = Color.black;

        showTimer = 0;
        showText.text = defaultText;

    }


    void Update()
    {
        if (Fighter == null) { Destroy(this); }
        else
        {
            if (showTimer != 0) { showTimer--; }
            if (showTimer == 0 && showText.text != defaultText) { showText.text = defaultText; showText.color = normalColor; }
        }
        //if (!selfBbutton.interactable) { selfBbutton.gameObject.SetActive(false); }
    }

    public void ShowDamage(int damage, bool isCrit)
    {
        string predamage = "HIT ";
        showText.color = normalColor;

        if (isCrit) { showText.color = critColor; predamage = "CRIT HIT "; }

        this.showText.text = (damage > 0) ? predamage + damage.ToString() : "MISS";
        showTimer = 200;
    }

    public void ShowHeal(int heal)
    {
        showText.color = healColor;
        this.showText.text = heal.ToString();
        showTimer = 200;
    }


    public void OnClick()
    {
        var combatManager = FindObjectOfType<CombatManager>();

        if (GameManager.Instance.ConfirmationClick)
        {
            if (combatManager.AttackWeapon != null && Fighter.CurrentHP > 0)
            {
                // se utiliza la función fight sobre el fighter al que corresponde el botón
                combatManager.Fight(this);
                // se termina el turno
                combatManager.ActionDone = true;

            }

            else
            {
                combatManager.SetlDescriptorText("Ivalid Action!");
                Debug.Log("Falta escoger el ataque o el arma que se utilizará o bien el enemigo está muerto");
            }


            if (combatManager.SelectedConsumible != null)
            {
                Debug.Log("Objeto usado -> " + combatManager.SelectedConsumible.name);
                // se activa la funcion en useo del consumible seleccionado, sobre el fighter
                // al que corresponde el botón
                Debug.Log("SaludAntes: " + Fighter.CurrentHP);
                combatManager.SelectedConsumible.OnUse(Fighter);
                Debug.Log("SaludDespues: " + Fighter.CurrentHP);

                // AQUI ACTUALIZAR BARRA
                Fighter.OnTakeDamage();

            }
        }

        GameManager.Instance.ConfirmationClick = false;

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string descripcion = "";
        string fighterName = "";
        var cm = FindObjectOfType<CombatManager>();
        cm.ClearPanelDescriptor();

        if (Fighter != null)
        {
            if ( cm.PlayerFighters.Contains(Fighter)) {fighterName = Fighter.RealName; }
            else { fighterName = Fighter.Name; }

            if (Fighter.CurrentHP <= 0)
            {
                descripcion = $" {fighterName} is dead";
                cm.SetlDescriptorText(descripcion);
            }
            else
            {
                cm.AddDamageTypeButton(Fighter.Type);
                descripcion = $" Name: {fighterName}\n Level: {Fighter.Level} \n Health: {Fighter.CurrentHP} / {Fighter.MaxHP}";
                cm.SetlDescriptorText(descripcion);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var cm = FindObjectOfType<CombatManager>();
        if(cm!=null)
        {
            cm.FillWithAttackWeapon();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var cm = FindObjectOfType<CombatManager>();
        if (cm.SelectedConsumible != null && cm.PlayerFighters.Contains(Fighter))
        {
            OnClick();
        }
        else if(!cm.PlayerFighters.Contains(Fighter) && cm.AttackWeapon != null)
        {
            OnClick();
        }
        
    }

}
