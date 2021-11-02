using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Text;

public class InfoBox : MonoBehaviour
{
    public TextMeshProUGUI Title;

    public GameObject WeaponFormatObj;
    
    public TextMeshProUGUI WeaponBaseDamageText;
    public TextMeshProUGUI WeaponTypeText;
    public TextMeshProUGUI WeaponStatusEffectsText;
    public TextMeshProUGUI WeaponDescriptionText;

    public GameObject GenericFormatObj;

    public TextMeshProUGUI GenericDescriptionText;

    void Start()
    {
        //Hide();
    }

    public void Clear()
    {
        Title.text = "";
        WeaponFormatObj.SetActive(false);
        GenericFormatObj.SetActive(false);
    }

    public void ShowInfo(string title, string description)
    {
        GenericFormatObj.SetActive(true);
        WeaponFormatObj.SetActive(false);

        Title.text = title;
        GenericDescriptionText.text = description;
    }
    public void ShowInfo(Item item)
    {
        Weapon itemAsWeapon = item as Weapon;
        //Consumible itemAsConsumible = item as Consumible;

        if (itemAsWeapon != null)
        {
            ShowInfo(itemAsWeapon);
        }
        else
        {
            GenericFormatObj.SetActive(true);
            WeaponFormatObj.SetActive(false);

            Title.text = item.Name;
            GenericDescriptionText.text = item.Description;
        }
    }

    public void ShowInfo(Weapon weapon)
    {
        GenericFormatObj.SetActive(false);
        WeaponFormatObj.SetActive(true);

        Title.text = weapon.Name;
        WeaponBaseDamageText.text = weapon.BaseDamage.ToString();
        WeaponTypeText.text = weapon.TipoDeDañoQueAplica.Name;

        string stringAplidedStates = "";
        foreach (var state in weapon.ListaDeEstadosQueAplica)
        {
            stringAplidedStates = stringAplidedStates + state.Name + "\n";
        }
        WeaponStatusEffectsText.text = stringAplidedStates;

        WeaponDescriptionText.text = weapon.Description;
    }

}
