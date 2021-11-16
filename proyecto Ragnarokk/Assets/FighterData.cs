using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


/// <summary>
/// esta clase tiene dos usos:
/// IDENTIFICAR LAS STATS Y TODAS LAS CARACTERISTICAS DE LOS COMBATIENTES. 
/// EL SISTEMA DE COMBATE DEBE SER CAPAZ DE SPAWNEAR UN COMBATIENTE SOLO CON ESTOS DATOS.
/// como cada combatiente va a tener unas estadisticas distintas que los demas ac� seria bueno poner toda la info relvante del luchador
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
    public bool IsMaxLevel = false;
    public FighterData[] Evolutions = new FighterData[2];

    public string Name = "unnamed fighter type";

    public string RealName = "nameless";

    public string Description = "Description";

    #region BASE STATS
    public int Speed = 10;
    public int Atack = 10;
    public int Defense = 5;
    public int Luck;
    public int MaxHP = 100;
    //public int CurrentHP = 100;
    #endregion

    [Range(0, 4)]
    public byte PowerRating = 1; //Valoraci�n subjetiva de qu� tan poderoso es un luchador (pensado para enemigos)
    
    public Weapon[] DefaultWeapons = new Weapon[4];

    #region VISUAL DATA
    public Sprite Sprite;
    public bool reversedSprite = false;
    public Vector2 size = new Vector2(1, 1);
    public Vector2 healthBarOffset = new Vector2(1,1);
    #endregion

    //Tipo para determinar que ataques hacen poco da�o y mucho da�o.
    public CombatType Type;
}
