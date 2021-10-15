using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_ChangeWeapon : MonoBehaviour
{

    public Weapon weapon;
    private UI_WeaponSelect weaponSelect;
    private UIManager uiManager;

    public void OnClick()
    {
        weaponSelect.SelectWeapon(weapon);
        uiManager.ReturnFromWeaponSelect();
    }

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>(true);
        weaponSelect = FindObjectOfType<UI_WeaponSelect>();
        GetComponentInChildren<TextMeshProUGUI>().text = weapon.Name;

        ColorBlock buttonCB = gameObject.GetComponent<Button>().colors;
        buttonCB.normalColor = weapon.TipoDeDañoQueAplica.Color;
        //buttonCB.highlightedColor = weapon.TipoDeDañoQueAplica.Color;
        gameObject.GetComponent<Button>().colors = buttonCB;
    }


}
