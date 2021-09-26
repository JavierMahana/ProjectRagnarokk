using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Combat/Type")]
public class CombatType : ScriptableObject
{
    public List<CombatState> Sinergias;
    public List<CombatState> AntiSinergias;

    public List<CombatType> Fortalezas;
    public List<CombatType> Debilidades;
}
