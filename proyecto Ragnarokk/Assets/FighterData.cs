using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


/// <summary>
/// esta clase tiene dos usos:
/// IDENTIFICAR LAS STATS Y TODAS LAS CARACTERISTICAS DE LOS COMBATIENTES. 
/// EL SISTEMA DE COMBATE DEBE SER CAPAZ DE SPAWNEAR UN COMBATIENTE SOLO CON ESTOS DATOS.
/// como cada combatiente va a tener unas estadisticas distintas que los demas acá seria bueno poner toda la info relvante del luchador
/// stats base:
/// vel, ataque, defensa, vida max
/// tipo
/// Armas por defecto
/// 
/// 
/// SERVIR COMO UN IDENTIFICADOR UNICO DEL TIPO DE CLASE QUE PERTENECE EL COMBATIENTE
/// 
/// 
/// </summary>
[CreateAssetMenu(menuName ="Fighter Data")]
public class FighterData : ScriptableObject
{
    public int ExpNeededToLevelUp = 100;
    public int Level = 0;
    public bool IsMaxLevel = false;
    public FighterData[] Evolutions = new FighterData[2];

    public string Name = "unnamed fighter type";

    public string RealName = "nameless";

    [Multiline]
    public string Description = "Description";

    public RuntimeAnimatorController FemaleController;
    public RuntimeAnimatorController MaleController;
    public RuntimeAnimatorController EnemyController;

    #region BASE STATS
    public int Speed = 10;
    public int Atack = 10;
    public int Defense = 5;
    public int Luck;
    public int MaxHP = 100;
    //public int CurrentHP = 100;
    #endregion

    [Range(0, 4)]
    public byte PowerRating = 1; //Valoración subjetiva de qué tan poderoso es un luchador (pensado para enemigos)
    
    public Weapon[] DefaultWeapons = new Weapon[4];

    #region VISUAL DATA
    public Sprite Sprite;
    public Sprite SpriteFemale;
    public Sprite SpriteMale;

    public bool reversedSprite = false;
    public Vector2 size = new Vector2(1, 1);
    public Vector2 healthBarOffset = new Vector2(1,1);
    #endregion

    //Tipo para determinar que ataques hacen poco daño y mucho daño.
    public CombatType Type;

    //Bonus de daño elemental al adquirir esta evolución
    public int WaterDamageBonus = 0;
    public int EarthDamageBonus = 0;
    public int FireDamageBonus = 0;
    public int AirDamageBonus = 0;
    public int PhysicalDamageBonus = 0;
    public int PsychicDamageBonus = 0;
    public int ElectricDamageBonus = 0;
    public int AlcoholDamageBonus = 0;

    //Bonus de resistencia elemental al adquirir esta evolución
    public int WaterResistanceBonus = 0;
    public int EarthResistanceBonus = 0;
    public int FireResistanceBonus = 0;
    public int AirResistanceBonus = 0;
    public int PhysicalResistanceBonus = 0;
    public int PsychicResistanceBonus = 0;
    public int ElectricResistanceBonus = 0;
    public int AlcoholResistanceBonus = 0;
}
