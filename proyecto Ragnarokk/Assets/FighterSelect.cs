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

    private TextAnimations animator1;
    private TextAnimations animator2;

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
        animator1 = animableObject1.GetComponent<TextAnimations>();
        animator2 = animableObject2.GetComponent<TextAnimations>();

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
        if (Fighter == null) { Destroy(this.gameObject); }
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

    
    public void ShowText(bool isDamage, bool isHope, int value, bool isCrit, int syn)
    {
        // syn 0 para cuando no hay sinergia alguna
        // syn -1 para cuando hay antisinergia
        // syn 1 para cuando hay sinergia

        BeginTextAnimation();

        if(isDamage)
        {
            string predamage = "Golpe ";
            string synergyText = "";
            showText.color = normalColor;

            if (isCrit) { showText.color = critColor; predamage = "¡GOLPE CRÍTICO! "; }

            switch (syn)
            {
                case 1:
                    synergyText = "!Sinergia!";
                    Debug.Log("hubo sinergia");
                    this.synergyText.color = synergyColor;
                    break;

                case -1:
                    synergyText = "...Anti-sinergia";
                    Debug.Log("hubo antisinergias");
                    this.synergyText.color = antiSynergyColor;
                    break;

                default:
                    Debug.Log("no hubo sinergias");
                    break;

            }

            showText.text = (value > 0) ? predamage + value.ToString() : "FALLO";
            this.synergyText.text = synergyText;
            showTimer = 400;
        }
        else if(isHope)
        {
            showText.text = "";
            synergyText.color = Color.white;
            this.synergyText.text = "+ " + value.ToString() + " Esperanza";
            showTimer = 400;

        }
        else
        {
            showText.text = "";
            this.synergyText.text = "+ " + value.ToString();
            showText.color = healColor;
            showTimer = 400;
        }
    }

    private void BeginTextAnimation()
    {
        animator1.AnimationStart();
        animator2.AnimationStart();
    }

    private void ResetTextAnimation()
    {
        animator1.AnimationReset();
        animator2.AnimationReset();
    }

    private void EndTextAnimation()
    {
        animator1.AnimationEnd();
        animator2.AnimationEnd();
    }

    public void OnClick()
    {
        ResetTextAnimation();

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
                    cm.AddSynergyButton(cm.AttackWeapon.TipoDeDañoQueAplica, Fighter);
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
