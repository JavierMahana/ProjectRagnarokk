using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WeaponSelect : MonoBehaviour
{

    public void Init(Fighter currFighter, int index)
    {
        if (index < 0 || index >= 4)
        {
            throw new System.ArgumentException("index debe ser un numero entre el 0 y 3");
        }
        this.index = index;
        this.currFighter = currFighter;
    }
    private int index;
    private Fighter currFighter;

    public void SelectWeapon(Weapon weapon)
    {
        currFighter.Weapons[index] = weapon;
    }
}
