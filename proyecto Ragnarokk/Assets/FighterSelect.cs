using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterSelect : MonoBehaviour
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
        if(Fighter == null) { Destroy(this); }
        if(showTimer !=0) { showTimer--;}
        if(showTimer == 0 && damage.text != defaultText) { damage.text = defaultText; }
        //if (!selfBbutton.interactable) { selfBbutton.gameObject.SetActive(false); }
    }

    public void ShowDamage(int damage)
    {
        this.damage.text = damage.ToString();
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
                combatManager.AttackDone = true;

            }

            else
            {
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

            }
        }
           
        GameManager.Instance.ConfirmationClick = false;

    }

}
