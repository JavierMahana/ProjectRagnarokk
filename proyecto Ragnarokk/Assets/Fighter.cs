using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Fighter : MonoBehaviour
{
    private SpriteRenderer spRenderer;
    [HideInInspector]
    public Animator animator;

    private void Awake()
    {
        spRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spRenderer == null)
        {
            Debug.LogError("Debe tener un hijo con sprite renderer!");
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Debe tener un hijo con animator!");
        }

        foreach (CombatType type in GameManager.Instance.AllCombatTypes)
        {
            TypeDamageBonuses.Add(type, 0);
            TypeResistanceBonuses.Add(type, 0);
        }
    }
    public void Init(FighterData data)
    {
        if(data.animatorController != null) { animator.runtimeAnimatorController = data.animatorController; }
        

        Name = data.Name;
        Atack = data.Atack;
        Defense = data.Defense;
        Speed = data.Speed;
        Luck = data.Luck;
        MaxHP = data.MaxHP;
        CurrentHP = MaxHP;

        PowerRating = data.PowerRating;

        Type = data.Type;

        //Añade bonus de daño elemental
        Dictionary<CombatType, int> typeDamageAddedBonuses = new Dictionary<CombatType, int>(GetNewTypeDamageBonuses(data));
        foreach (CombatType type in GameManager.Instance.AllCombatTypes)
        {
            TypeDamageBonuses[type] += typeDamageAddedBonuses[type];
        }

        //Añade bonus de resistencia elemental
        Dictionary<CombatType, int> typeResistanceAddedBonuses = new Dictionary<CombatType, int>(GetNewTypeResistanceBonuses(data));
        foreach (CombatType type in GameManager.Instance.AllCombatTypes)
        {
            TypeResistanceBonuses[type] += typeResistanceAddedBonuses[type];
        }

        Size = data.healthBarOffset;
        reversed = data.reversedSprite;

        Evolutions = data.Evolutions;
        CurrentExp = 0;
        ExpNeededToLevelUp = data.ExpNeededToLevelUp;
        IsMaxLevel = data.IsMaxLevel;

        Sprite = data.Sprite;

        spRenderer.sprite = Sprite;

        if(Level == 0)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i] = data.DefaultWeapons[i];
                WeaponCooldowns[i] = 0;
            }
        }
        
        SpriteFemale = data.SpriteFemale;
        SpriteMale = data.SpriteMale;

        Level++;

        /*
        Debug.Log($"Bonus ataque {Name}:");
        foreach(CombatType type in GameManager.Instance.AllCombatTypes)
        {
            Debug.Log($"{type.Name}: {TypeDamageBonuses[type]}");
        }

        Debug.Log($"Bonus resistencia {Name}:");
        foreach (CombatType type in GameManager.Instance.AllCombatTypes)
        {
            Debug.Log($"{type.Name}: {TypeResistanceBonuses[type]}");
        }
        */
        /*
        GameObject fPanel = Instantiate(GameManager.Instance.PrefabFighterSelect, this.transform);
        fPanel.GetComponent<FighterSelect>().Fighter = this;
        gameObject.AddComponent<FighterSelect>();
        var fs = gameObject.GetComponent<FighterSelect>();
        fs.Fighter = this;
        fs.showText = fPanel.GetComponent<FighterSelect>().showText;*/
    }

    private Dictionary<CombatType, int> GetNewTypeDamageBonuses(FighterData data)
    {
        Dictionary<CombatType, int> bonuses = new Dictionary<CombatType, int>();

        foreach(CombatType type in GameManager.Instance.AllCombatTypes)
        {
            switch(type.Name)
            {
                case "Water"    : bonuses.Add(type, data.WaterDamageBonus);       break;
                case "Earth"    : bonuses.Add(type, data.EarthDamageBonus);       break;
                case "Fire"     : bonuses.Add(type, data.FireDamageBonus);        break;
                case "Air"      : bonuses.Add(type, data.AirDamageBonus);         break;
                case "Physical" : bonuses.Add(type, data.PhysicalDamageBonus);    break;
                case "Psychic"  : bonuses.Add(type, data.PsychicDamageBonus);     break;
                case "Electric" : bonuses.Add(type, data.ElectricDamageBonus);    break;
                case "Alcohol"  : bonuses.Add(type, data.AlcoholDamageBonus);     break;
            }
        }

        return bonuses;
    }

    private Dictionary<CombatType, int> GetNewTypeResistanceBonuses(FighterData data)
    {
        Dictionary<CombatType, int> bonuses = new Dictionary<CombatType, int>();

        foreach (CombatType type in GameManager.Instance.AllCombatTypes)
        {
            switch (type.Name)
            {
                case "Water"    : bonuses.Add(type, data.WaterResistanceBonus);     break;
                case "Earth"    : bonuses.Add(type, data.EarthResistanceBonus);     break;
                case "Fire"     : bonuses.Add(type, data.FireResistanceBonus);      break;
                case "Air"      : bonuses.Add(type, data.AirResistanceBonus);       break;
                case "Physical" : bonuses.Add(type, data.PhysicalResistanceBonus);  break;
                case "Psychic"  : bonuses.Add(type, data.PsychicResistanceBonus);   break;
                case "Electric" : bonuses.Add(type, data.ElectricResistanceBonus);  break;
                case "Alcohol"  : bonuses.Add(type, data.AlcoholResistanceBonus);   break;
            }
        }

        return bonuses;
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
    public Dictionary<CombatType, int> TypeDamageBonuses = new Dictionary<CombatType, int>();
    public Dictionary<CombatType, int> TypeResistanceBonuses = new Dictionary<CombatType, int>();

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
