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

        if (GameManager.Instance.ConfirmationClick && combatManager.AttackWeapon != null)
        {  
            combatManager.Fight(this);
            combatManager.AttackDone = true;
            GameManager.Instance.ConfirmationClick = false;
        }
        else
        {
            Debug.Log("Falta escoger el ataque o el arma que se utilizará");
        }

    }

}
