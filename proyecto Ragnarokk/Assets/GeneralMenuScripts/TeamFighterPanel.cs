using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamFighterPanel : MonoBehaviour
{
    public Image thisimage;

    public Text healthPosition;
    public Text FighterName;
    public Text ValueAttack;
    public Text ValueDefense;
    public Text ValueSpeed;

    public void fillPanel(Fighter f)
    {
        thisimage.sprite = f.GetComponent<SpriteRenderer>().sprite;

        //añador el level luego del nombre, en el mismo string
        FighterName.text = f.Name;
        healthPosition.text = f.CurrentHP.ToString() + " / " + f.MaxHP.ToString();
        ValueAttack.text = f.Atack.ToString();
        ValueDefense.text = f.Defense.ToString();
        ValueSpeed.text = f.Defense.ToString();
    }
}
