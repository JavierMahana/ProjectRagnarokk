using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamFighterPanel : MonoBehaviour
{
    public Image thisimage;

    public TextMeshProUGUI healthPosition;
    public TextMeshProUGUI FighterName;
    public TextMeshProUGUI ValueLevel;
    public TextMeshProUGUI ValueType;
    public TextMeshProUGUI ValueAttack;
    public TextMeshProUGUI ValueDefense;
    public TextMeshProUGUI ValueSpeed;
    public TextMeshProUGUI ValueLuck;

    public void fillPanel(Fighter f)
    {
        thisimage.sprite = f.Sprite;
        thisimage.preserveAspect = true;
        //a�ador el level luego del nombre, en el mismo string
        FighterName.text = f.RealName;
        if (f.CurrentHP == 0)
        {
            healthPosition.text = "<#3e3e3e>" + f.CurrentHP.ToString() + "</color>" + " / " + f.MaxHP.ToString();
        }
        else if (f.CurrentHP == f.MaxHP)
        {
            healthPosition.text = "<#00FF00>" + f.CurrentHP.ToString() + "</color>" + " / " + f.MaxHP.ToString();
        }
        else
        {
            healthPosition.text = "<#C0C0C0>" + f.CurrentHP.ToString() + "</color>" + " / " + f.MaxHP.ToString();
        }

       
        ValueLevel.text = f.Level.ToString();
        ValueType.text = f.Type.name;
        ValueType.color = f.Type.Color;
        ValueAttack.text = f.Atack.ToString();
        ValueDefense.text = f.Defense.ToString();
        ValueSpeed.text = f.Speed.ToString();
        ValueLuck.text = f.Luck.ToString();
       
    }
}
