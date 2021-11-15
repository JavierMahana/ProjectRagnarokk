using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


/// <summary>
/// Este objeto sirve como traductor del sistema de Game y el de Combate.
/// el sistema de game solo le entrega un encuentro al sistema de combate y este spawnea los enemigos correspondientes.
/// </summary>
[CreateAssetMenu(menuName = "Combat/Combat Encounter")]
public class CombatEncounter : ScriptableObject
{
    public List<FighterData> ListOfEncounterEnemies = new List<FighterData>();

    public int ExpReward = 100;
    public int MoneyReward = 25;
    public bool CanEscape = true;
}
