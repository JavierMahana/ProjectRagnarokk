using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Combat/Type")]
public class CombatType : ScriptableObject
{
    public string Name = "unnamed type";

    public Color Color;

    public List<CombatState> Sinergias;
    public List<CombatState> AntiSinergias;

    public List<CombatType> Fortalezas;
    public List<CombatType> Debilidades;
}
