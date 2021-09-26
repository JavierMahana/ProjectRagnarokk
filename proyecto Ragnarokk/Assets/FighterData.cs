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
    
    public string Name = "unnamed fighter type";

    #region BASE STATS
    public int Speed = 10;
    public int Atack = 10;
    public int Defense = 5;
    public int MaxHP = 100;
    #endregion
    
    public Weapon[] DefaultWeapons = new Weapon[4];

    #region VISUAL DATA
    public Sprite Sprite;
    #endregion

    //Tipo para determinar que ataques hacen poco daño y mucho daño.
    public CombatType Type;
}
