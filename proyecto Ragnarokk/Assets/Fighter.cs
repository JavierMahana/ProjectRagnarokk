using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Fighter : MonoBehaviour
{
    private SpriteRenderer spRenderer;
    private void Awake()
    {
        spRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spRenderer == null)
            Debug.LogError("Debe tener un hijo con sprite renderer!");
    }
    public void Init(FighterData data)
    {
        Name = data.Name;
        Atack = data.Atack;
        Defense = data.Defense;
        Speed = data.Speed;
        Luck = data.Luck;
        MaxHP = data.MaxHP;
        CurrentHP = MaxHP;

        PowerRating = data.PowerRating;

        Type = data.Type;

        Size = data.healthBarOffset;
        reversed = data.reversedSprite;

        Evolutions = data.Evolutions;
        CurrentExp = 0;
        ExpNeededToLevelUp = data.ExpNeededToLevelUp;
        IsMaxLevel = data.IsMaxLevel;

        Sprite = data.Sprite;

        spRenderer.sprite = Sprite;

        for (int i = 0; i < Weapons.Length; i++)
        {
            Weapons[i] = data.DefaultWeapons[i];
            WeaponCooldowns[i] = 0;
        }

        SpriteFemale = data.SpriteFemale;
        SpriteMale = data.SpriteMale;

        Level++;
    }

    [ReadOnly]
    public Sprite Sprite;

    public Sprite SpriteFemale;
    public Sprite SpriteMale;

    [ReadOnly]
    public int CurrentExp;
    [ReadOnly]
    public int ExpNeededToLevelUp;
    [ReadOnly]
    public bool IsMaxLevel;

    public int Level;

    [ReadOnly]
    public FighterData[] Evolutions = new FighterData[2];


    [ReadOnly]
    public string Name;

    [ReadOnly]
    public int Speed;
    [ReadOnly]
    public int Atack;
    [ReadOnly]
    public int Defense;
    [ReadOnly]
    public int Luck;
    [ReadOnly]
    public int MaxHP;
    [ReadOnly]
    public int CurrentHP;

    public byte PowerRating;

    [ReadOnly]
    public Weapon[] Weapons = new Weapon[4];
    public int[] WeaponCooldowns = new int[4];

    public Weapon CurrentWeapon;

    public CombatType Type;

    public List<CombatState> States = new List<CombatState>();

    public string RealName;

    public bool isMale;

    public bool IsDefending = false;

    public bool reversed;
    public Vector2 Size = new Vector2(1,1);

    public VoidDelegate OnTakeDamage;
    public VoidDelegate OnDie;



    //definicion de tipo de delegate
    public delegate void VoidDelegate();


}
