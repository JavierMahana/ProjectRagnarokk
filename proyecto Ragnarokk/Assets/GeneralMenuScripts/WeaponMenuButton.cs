using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenuButton : MonoBehaviour
{
    public Weapon thisWeapon;
    public Text thisWeaponName;
    public GameObject selfButton;

    public Image thisImage;

    private Image defaultImage;
    private Color defaultColor;

    public void Start()
    {
        defaultColor = Color.clear;
    }

    public void FillWeaponButton(Fighter f, int index)
    {
        thisWeapon = f.Weapons[index];
        UpdateButton();
       
    }

    public void UpdateButton()
    {
        if(thisWeapon == null)
        {
            thisWeaponName.text = "Empty";
            thisImage.color = defaultColor;

        }
        else
        {
            thisImage.sprite = thisWeapon.sprite;
            thisImage.color = Color.white;
            thisWeaponName.text = "";
        }
    }
}

