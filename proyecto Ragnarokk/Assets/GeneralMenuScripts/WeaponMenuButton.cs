using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Weapon thisWeapon;
    public TextMeshProUGUI thisWeaponName;
    public Image thisImage;
    public Color defaultColor;

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
        var wp = FindObjectOfType<WeaponsPanel>();
        

        if (thisWeapon == null)
        {
            thisWeaponName.text = "Empty";
            thisImage.color = Color.clear;
            thisImage.sprite = null;
            GetComponent<Image>().color = defaultColor;
        }
        else
        {
            thisWeaponName.text = "";
            thisImage.sprite = thisWeapon.IconSprite;
            thisImage.preserveAspect = true;
            thisImage.color = defaultColor;
            if (wp.currentWeaponButton == this)
            {
                GetComponent<Image>().color = GetComponent<Button>().colors.highlightedColor;
            }
            else
            {
                GetComponent<Image>().color = defaultColor;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var wp = FindObjectOfType<WeaponsPanel>();
        if(wp.OnChange)
        {
            wp.UpdateWeaponPanel(this);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var wp = FindObjectOfType<WeaponsPanel>();
        if (wp.OnChange)
        {
            wp.UpdateWeaponPanel(null);
        }
    }



}

