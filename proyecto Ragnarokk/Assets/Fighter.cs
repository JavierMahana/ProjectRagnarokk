using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Fighter : MonoBehaviour
{
    [ReadOnly]
    public string Name;

    [ReadOnly]
    public int Speed;
    [ReadOnly]
    public int Atack;
    [ReadOnly]
    public int Defense;
    [ReadOnly]
    public int MaxHP;
    [ReadOnly]
    public int CurrentHP;

    [ReadOnly]
    public Weapon[] Weapons = new Weapon[4];

    public Weapon CurrentWeapon;

    public CombatType Type;

    public List<CombatState> States = new List<CombatState>();



    public VoidDelegate OnTakeDamage;
    public VoidDelegate OnDie;



    //definicion de tipo de delegate
    public delegate void VoidDelegate();


}
