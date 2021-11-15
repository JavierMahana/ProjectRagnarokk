using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System.Text;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public TextMeshProUGUI Title;

    public GameObject WeaponFormatObj;
    
    public TextMeshProUGUI WeaponBaseDamageText;
    public TextMeshProUGUI WeaponTypeText;
    public TextMeshProUGUI WeaponStatusEffectsText;
    public TextMeshProUGUI WeaponDescriptionText;

    public GameObject FighterFormatObj;

    public TextMeshProUGUI AtackText;
    public TextMeshProUGUI DefenceText;
    public TextMeshProUGUI SpeedText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI TypeText;
    public TextMeshProUGUI FighterDescriptionText;
    public Image FighterImage; 

    public GameObject GenericFormatObj;

    public TextMeshProUGUI GenericDescriptionText;

    void Start()
    {
        Clear();
    }

    public void Clear()
    {
        Title.text = "";
        WeaponFormatObj.SetActive(false);
        GenericFormatObj.SetActive(false);
        FighterFormatObj.SetActive(false);
    }

    public void ShowInfo(string title, string description)
    {
        FighterFormatObj.SetActive(false);
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
            FighterFormatObj.SetActive(false);
            GenericFormatObj.SetActive(true);
            WeaponFormatObj.SetActive(false);

            Title.text = item.Name;
            GenericDescriptionText.text = item.Description;
        }
    }

    public void ShowInfo(Weapon weapon)
    {
        FighterFormatObj.SetActive(false);
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

    public void ShowInfo(FighterData fighterData)
    {
        FighterFormatObj.SetActive(true);
        GenericFormatObj.SetActive(false);
        WeaponFormatObj.SetActive(false);

        Title.text = fighterData.Name;
        FighterDescriptionText.text = fighterData.Description;
        AtackText.text = fighterData.Atack.ToString();
        DefenceText.text = fighterData.Defense.ToString();
        SpeedText.text = fighterData.Speed.ToString();
        HPText.text = fighterData.MaxHP.ToString();
        TypeText.text = fighterData.Type.Name;

        FighterImage.sprite = fighterData.Sprite;
    }

}
