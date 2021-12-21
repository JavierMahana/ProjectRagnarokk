using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Combat/CombatState")]
public class CombatState : ScriptableObject
{
    //ESTO ES SOLO UN TAG.
    public string Name = "unnamed state";
    public Color color;
    public Sprite Sprite;
}
