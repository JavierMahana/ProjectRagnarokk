using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string Name = "";

    // esto es para las iteraciones de armas
    // por ejemplo, Ak47 +1, porque el AudioManager utilizan el Name
    // para encontrar el sonido correspondiente a un arma

    public string SubName = "";
    public string Description = "";

    public int BaseCost = 100;
    public Sprite sprite;

    public delegate void ItemDelegate(Item item);
}
