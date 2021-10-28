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


    public void Hide()
    {
        Title.text = "";
        WeaponFormatObj.SetActive(false);
    }

    public void ShowInfo(Weapon weapon)
    {
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
