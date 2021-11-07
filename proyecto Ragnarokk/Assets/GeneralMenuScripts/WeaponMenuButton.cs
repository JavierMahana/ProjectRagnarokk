using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponMenuButton : MonoBehaviour
{
    public Weapon thisWeapon;
    public Text thisWeaponName;
    public Image thisImage;

    private Color defaultColor;

    private void Start()
    {
        defaultColor = GetComponent<Button>().colors.normalColor;
    }

    public void FillWeaponButton(Fighter f, int index)
    {       
        thisWeapon = f.Weapons[index];
    }

    public void UpdateButton()
    {
        if(thisWeapon == null)
        {
            thisWeaponName.text = "Empty";
            thisImage.color = Color.clear;
            thisImage.sprite = null;
        }
        else
        {
            thisWeaponName.text = "";
            thisImage.sprite = thisWeapon.sprite;
            thisImage.color = defaultColor;
            GetComponent<Image>().color = defaultColor;
        }
    }
}

