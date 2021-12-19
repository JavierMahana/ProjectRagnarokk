using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateButton : MonoBehaviour
{
    public CombatState thisState;
    public Image thisImage;
    void Start()
    {
        thisImage.sprite = thisState.Sprite;
        thisImage.preserveAspect = true;
    }

}
