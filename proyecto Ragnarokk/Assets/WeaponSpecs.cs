using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WeaponSpecs : MonoBehaviour
{
    public Text weaponName;
    public Text weaponDamage;
    public Weapon thisWeapon;

    private Color selectColor = Color.blue;
    private Color initialColor = Color.white;

    [HideInInspector]
    public bool ButtonPressed;

    private void Start()
    {
        ButtonPressed = false;
    }

    public WeaponSpecs(string name, string damage, Weapon weapon)
    {
        weaponName.text = name;
        weaponDamage.text = damage + "dmg";
        thisWeapon = weapon;
    }
    public void OnClick()
    {
        CombatManager combatManager = GameObject.Find("CombatManager").GetComponent<CombatManager>();
        

        if (!ButtonPressed)
        {
            GameManager.Instance.ConfirmationClick = true;
            combatManager.AttackWeapon = thisWeapon;
            ButtonPressed = true;
        }
        else
        {
            combatManager.AttackWeapon = null;
            ButtonPressed = false;
        }
    }
}
