using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string Name = "";

    public string SubName = "";
    [Multiline]
    public string Description = "";

    public int BaseCost = 100;
    public Sprite IconSprite;

    public delegate void ItemDelegate(Item item);
}
