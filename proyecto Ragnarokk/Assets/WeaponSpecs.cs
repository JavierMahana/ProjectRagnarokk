using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class WeaponSpecs : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI synergyCount;
    //public Text weaponDamage;
    public Weapon thisWeapon;

    [HideInInspector]
    public int IndexOfFighterWeapon;

    public Color defaultColor;

    [HideInInspector]
    public bool ButtonPressed;

    private void Start()
    {
        ButtonPressed = false;
        defaultColor = ColorBlock.defaultColorBlock.normalColor;
    }

    public WeaponSpecs(string name, string damage, Weapon weapon, int weaponIndex)
    {
        weaponName.text = name;
        //weaponDamage.text = damage + "dmg";
        thisWeapon = weapon;
        IndexOfFighterWeapon = weaponIndex;
    }
    public void OnClick()
    {
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();

        int weaponCooldown = combatManager.ActiveFighter.WeaponCooldowns[IndexOfFighterWeapon];
        Debug.Log("Cooldown del arma: " + weaponCooldown);
        if (!ButtonPressed && combatManager.AttackWeapon != thisWeapon && weaponCooldown <= 0)
        {
            GameManager.Instance.ConfirmationClick = true;
            combatManager.AttackWeapon = thisWeapon;
            combatManager.AttackWeaponIndex = IndexOfFighterWeapon; //Se usará para asignar el cooldown al arma correcta
            foreach (GameObject button in combatManager.AllButtonsInPanel)
            {
                if (button.TryGetComponent(out WeaponSpecs ws))
                {
                    ws.ButtonPressed = false;
                }
            }
            ButtonPressed = true;
        }
        else
        {
            GameManager.Instance.ConfirmationClick = false;
            combatManager.AttackWeapon = null;
            GetComponent<Image>().color = defaultColor;
            ButtonPressed = false;
        }

        
    }

    public void FillDescriptorPanel()
    {
        var cm = FindObjectOfType<CombatManager>();

        cm.ClearPanelDescriptor();
        cm.AddDamageTypeButton(thisWeapon.TipoDeDañoQueAplica);
        string description = $" Precisión: {thisWeapon.BaseAccuracy} \n Daño Base: {thisWeapon.BaseDamage} \n Crítico: {thisWeapon.BaseCriticalRate} \n Enfriamiento: {thisWeapon.BaseCooldown}";
        cm.SetlDescriptorText(description);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        FillDescriptorPanel();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var cm = FindObjectOfType<CombatManager>();
        // aún no se escogio el arma no se limpia el panel de informacion 
        if(cm.AttackWeapon == null) { cm.ClearPanelDescriptor(); }
        else { cm.FillWithAttackWeapon(); }    

       
        
    }
}
