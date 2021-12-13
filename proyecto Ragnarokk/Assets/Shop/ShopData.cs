using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopData
{
    public Weapon weapon1;
    public Weapon weapon2;
    public Weapon weapon3;
    public Consumible consumible1;
    public Consumible consumible2;
    public Consumible consumible3;
    public ShopData(Weapon weapon1, Weapon weapon2, Weapon weapon3, Consumible consumible1, Consumible consumible2, Consumible consumible3)
    {
        UpdateContent(weapon1, weapon2, weapon3, consumible1, consumible2, consumible3);
    }
    public void UpdateContent(Weapon weapon1, Weapon weapon2, Weapon weapon3, Consumible consumible1, Consumible consumible2, Consumible consumible3)
    {
        this.weapon1 = weapon1;
        this.weapon2 = weapon2;
        this.weapon3 = weapon3;
        this.consumible1 = consumible1;
        this.consumible2 = consumible2;
        this.consumible3 = consumible3;
    }
    public void UpdateContent(int soldIndex)
    {
        switch(soldIndex)
        {
            case 0: weapon1 = null; break;
            case 1: weapon2 = null; break;
            case 2: weapon3 = null; break;
            case 3: consumible1 = null; break;
            case 4: consumible2 = null; break;
            case 5: consumible3 = null; break;
            default: Debug.LogWarning("Sold item index is not valid!"); break;
        }
    }
}
