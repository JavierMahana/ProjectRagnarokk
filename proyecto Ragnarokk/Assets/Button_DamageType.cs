using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Button_DamageType : MonoBehaviour
{
    public CombatType dmg;
    public TextMeshProUGUI dmgType;

    public void SetButton(CombatType type)
    {
        dmg = type;
        dmgType.text = type.name;
        GetComponent<Image>().color = type.Color;
    }

    public void Start()
    {
        if(dmg != null)
        {
            SetButton(dmg);
            var fab = GetComponent<ForAllButtons>();
            if(fab != null)
            {
                fab.Normal = dmg.Color;
                fab.Pressed = dmg.Color;
            }
        }
    }
}
