using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterSelect : MonoBehaviour
{
    public Button selfBbutton;

    public GameObject Fighter;

    public Text damage;

    public int showTimer;

    private string defaultText = "";
    //agregar despues una lista de imagenes (estados) dentro de un canvas (atributo) que se activa si hay efectos en Fighter

    void Start()
    {
        selfBbutton.interactable = false;
        showTimer = 0;
        damage.text = defaultText;
    }


    void Update()
    {
        if(Fighter == null) { Destroy(this); }
        if(showTimer !=0) { showTimer--; }
        if(showTimer == 0 && damage.text != defaultText) { damage.text = defaultText; }
        if (!selfBbutton.interactable) { selfBbutton.gameObject.SetActive(false); }
    }

    public void InflictDamage()
    {
        if(selfBbutton.interactable)
        {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            var pcOnTurn = gameManager.PlayerOnTurn;

            //Calculo de daño -- aqui--
            int dañoPC = pcOnTurn.GetComponent<Fighter>().Atack;
            int defensaEnemigo = Fighter.GetComponent<Fighter>().Defense;

            int dañoFinal = dañoPC - (int)(defensaEnemigo * 0.25);

            if(dañoFinal >= 0)
            {
                Debug.Log("Salud de enemigo antes del ataque: " + Fighter.GetComponent<Fighter>().CurrentHP);
                Fighter.GetComponent<Fighter>().CurrentHP -= dañoFinal;
                damage.text = dañoFinal.ToString();
                showTimer = 200;

                Debug.Log("Daño infligido: " + dañoFinal);
                Debug.Log("Salud de enemigo despues del ataque: " + Fighter.GetComponent<Fighter>().CurrentHP);
            }

            if(Fighter.GetComponent<Fighter>().CurrentHP <= 0)
            {
                Fighter.transform.rotation = new Quaternion(0, 0, 90, 0);
            }

            CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
            combatManager.AttackDone = true;
        }
       
    }
}
