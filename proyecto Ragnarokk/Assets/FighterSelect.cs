using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FighterSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button selfBbutton;

    public Fighter Fighter;

    public Text damage;

    public int showTimer;

    private string defaultText = "";

    //agregar despues una lista de imagenes (estados) dentro de un canvas (atributo) que se activa si hay efectos en Fighter

    void Start()
    {
        selfBbutton.interactable = true;
        showTimer = 0;
        damage.text = defaultText;
    }


    void Update()
    {
        if (Fighter == null) { Destroy(this); }
        if (showTimer != 0) { showTimer--; }
        if (showTimer == 0 && damage.text != defaultText) { damage.text = defaultText; }
        //if (!selfBbutton.interactable) { selfBbutton.gameObject.SetActive(false); }
    }

    public void ShowDamage(int damage)
    {
        this.damage.text = (damage > 0) ? damage.ToString() : "MISS";
        showTimer = 200;
    }

    public void OnClick()
    {
        var combatManager = FindObjectOfType<CombatManager>();

        if (GameManager.Instance.ConfirmationClick)
        {
            if (combatManager.AttackWeapon != null && Fighter.CurrentHP > 0)
            {
                // se utiliza la funci�n fight sobre el fighter al que corresponde el bot�n
                combatManager.Fight(this);
                // se termina el turno
                combatManager.ActionDone = true;

            }

            else
            {
                combatManager.SetlDescriptorText("Ivalid Action!");
                Debug.Log("Falta escoger el ataque o el arma que se utilizar� o bien el enemigo est� muerto");
            }


            if (combatManager.SelectedConsumible != null)
            {
                Debug.Log("Objeto usado -> " + combatManager.SelectedConsumible.name);
                // se activa la funcion en useo del consumible seleccionado, sobre el fighter
                // al que corresponde el bot�n
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

}
