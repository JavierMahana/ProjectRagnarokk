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
        thisimage.sprite = f.GetComponentInChildren<SpriteRenderer>().sprite;

        //añador el level luego del nombre, en el mismo string
        FighterName.text = f.RealName;
        healthPosition.text = f.CurrentHP.ToString() + " / " + f.MaxHP.ToString();
        ValueLevel.text = f.Level.ToString();
        ValueType.text = f.Type.Name;
        ValueAttack.text = f.Atack.ToString();
        ValueDefense.text = f.Defense.ToString();
        ValueSpeed.text = f.Speed.ToString();
        ValueLuck.text = f.Luck.ToString();
       
    }
}
