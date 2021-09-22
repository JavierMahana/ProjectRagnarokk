using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//luego las armas deben tener mas caracteristicas.
[CreateAssetMenu(menuName = "Combat/Weapon")]
public class Weapon : ScriptableObject
{
    public string Name = "unnamed weapon";
    public int BaseDamage = 50;
}
