using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_WeaponSlot : MonoBehaviour
{
    private Button button;
    private TextMeshProUGUI text;


    private Fighter currFighter;
    private int weaponIndex;
    private Weapon currWeapon;
    public void Init(Weapon weapon, Fighter fighter, int index)
    {
        weaponIndex = index;
        currFighter = fighter;

        if (button == null || text == null)
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }


        bool haveWeapon = weapon != null;
        //Debug.Log($"Init. has weapon: {haveWeapon}");
        if (weapon != null)
        {
            currWeapon = weapon;
            text.text = weapon.Name;
        }
        else 
        {
            currWeapon = null;
            text.text = "";
        }
    }

    private UIManager uiManager;
    private UI_WeaponSelect weaponSelect;

    private void Awake()
    {
        if (button == null || text == null)
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<TextMeshProUGUI>();
        }


        uiManager = FindObjectOfType<UIManager>(true);
        if (uiManager == null)
        {
            Debug.LogError("ADD UIMANAGER.");
            return;
        }

        weaponSelect = FindObjectOfType<UI_WeaponSelect>(true);
        if (weaponSelect == null)
        {
            Debug.LogError("ADD WEAPON SELECT PANEL. WITH THE WEAPON SELECT COMPONENT.");
            return;
        }

        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        uiManager.StartWeaponSelect();
        weaponSelect.Init(currFighter, weaponIndex);

        //if (currWeapon != null)
        //{
        //    Debug.Log($"Fighter:{currFighter.Name}| Weapon slot clicked! weapon name: {currWeapon.Name}");
        //}
        //else 
        //{
        //    Debug.Log($"Fighter:{currFighter.Name}| Weapon slot clicked! No weapon asigned yet.");
        //}
    }
}
