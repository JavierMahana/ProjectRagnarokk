using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_DamageType : MonoBehaviour
{
    public TextMeshProUGUI dmgType;

    public void SetButton(CombatType type)
    {
        dmgType.text = type.name;
        GetComponent<Image>().color = type.Color;
    }
}
