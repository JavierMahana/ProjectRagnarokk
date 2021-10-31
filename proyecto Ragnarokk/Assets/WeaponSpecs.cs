using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSpecs : MonoBehaviour
{


    public Text weaponName;
    public Text weaponDamage;
    public Weapon thisWeapon;

    [HideInInspector]
    public int IndexOfFighterWeapon;

    private Color selectColor = Color.blue;
    [HideInInspector]
    public Color initialColor = Color.white;

    [HideInInspector]
    public bool ButtonPressed;

    private void Start()
    {
        ButtonPressed = false;
    }

    public WeaponSpecs(string name, string damage, Weapon weapon, int weaponIndex)
    {
        weaponName.text = name;
        weaponDamage.text = damage + "dmg";
        thisWeapon = weapon;
        IndexOfFighterWeapon = weaponIndex;
    }
    public void OnClick()
    {
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();

        int weaponCooldown = combatManager.ActiveFighter.WeaponCooldowns[IndexOfFighterWeapon];
        Debug.Log("Cooldown del arma: " + weaponCooldown);
        if(weaponCooldown <= 0)
        {
            if (!ButtonPressed)
            {
                GameManager.Instance.ConfirmationClick = true;
                combatManager.AttackWeapon = thisWeapon;
                combatManager.AttackWeaponIndex = IndexOfFighterWeapon;
                ButtonPressed = true;
            }
            else
            {
                combatManager.AttackWeapon = null;
                ButtonPressed = false;
            }
        }
    }
}
