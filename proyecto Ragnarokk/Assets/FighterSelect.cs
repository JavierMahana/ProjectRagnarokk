using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FighterSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject animableObject1;
    public GameObject animableObject2;

    public Fighter Fighter;
    
    public TextMeshProUGUI showText;
    public TextMeshProUGUI synergyText;

    public int showTimer;

    private string defaultText = "";
    private Color normalColor;
    private Color healColor;
    private Color critColor;

    private Color synergyColor;
    private Color antiSynergyColor;


    //agregar despues una lista de imagenes (estados) dentro de un canvas (atributo) que se activa si hay efectos en Fighter

    void Start()
    {
        float r = 0;
        float g = 0;
        float b = 0;

        r = 1f;
        g = 120 / 255f;
        b = 0;
        critColor = new Color(r, g, b, 1f);

        r = 0;
        g = 200 / 255f;
        b = 130 / 255f;
        healColor = new Color(r, g, b, 1f);

        antiSynergyColor = Color.red;
        synergyColor = healColor;

        normalColor = showText.color;

        showTimer = 0;
        showText.text = defaultText;

    }


    void Update()
    {
        if (Fighter == null) { Destroy(this); }
        else
        {
            if (showTimer != 0) 
            {
                showTimer--; 
            }
            if (showTimer == 0 && showText.text != defaultText) 
            {
                showText.text = defaultText; 
                showText.color = normalColor;

                synergyText.text = defaultText;
                synergyText.color = synergyColor;

                EndTextAnimation();
            }
        }
    }

    
    public void ShowText(bool isDamage, int value, bool isCrit, int syn)
    {
        // syn 0 para cuando no hay sinergia alguna
        // syn -1 para cuando hay antisinergia
        // syn 1 para cuando hay sinergia

        BeginTextAnimation();

        if(isDamage)
        {
            string predamage = "HIT ";
            string synergyText = "";
            showText.color = normalColor;

            if (isCrit) { showText.color = critColor; predamage = "CRIT HIT "; }

            switch (syn)
            {
                case 1:
                    synergyText = "Synergy!";
                    Debug.Log("hubo sinergia");
                    this.synergyText.color = synergyColor;
                    break;

                case -1:
                    synergyText = "AntiSynergy";
                    Debug.Log("hubo antisinergias");
                    this.synergyText.color = antiSynergyColor;
                    break;

                default:
                    Debug.Log("no hubo sinergias");
                    break;

            }

            showText.text = (value > 0) ? predamage + value.ToString() : "MISS";
            this.synergyText.text = synergyText;
            showTimer = 400;
        }
        else
        {
            showText.text = "";
            this.synergyText.text = "+ " + value.ToString() + "!"; ;
            showText.color = healColor;
            showTimer = 400;
        }
    }

    private void BeginTextAnimation()
    {
        animableObject1.GetComponent<TextAnimations>().AnimationStart();
        animableObject2.GetComponent<TextAnimations>().AnimationStart();
    }

    private void ResetTextAnimation()
    {
        animableObject1.GetComponent<TextAnimations>().AnimationReset();
        animableObject2.GetComponent<TextAnimations>().AnimationReset();
    }

    private void EndTextAnimation()
    {
        animableObject1.GetComponent<TextAnimations>().AnimationReset();
        animableObject2.GetComponent<TextAnimations>().AnimationReset();
    }

    public void OnClick()
    {
        ResetTextAnimation();

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
                combatManager.SelectedConsumible.OnUse(Fighter, this);
                Debug.Log("SaludDespues: " + Fighter.CurrentHP);

                // AQUI ACTUALIZAR BARRA -- 
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

                if(cm.AttackWeapon != null)
                {
                    cm.AddSynergyButton(cm.AttackWeapon.TipoDeDa�oQueAplica, Fighter);
                }
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
