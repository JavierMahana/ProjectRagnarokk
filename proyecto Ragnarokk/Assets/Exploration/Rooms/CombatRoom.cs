using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Combat Room")]
public class CombatRoom : RoomData
{
    public CombatEncounter encounter;


    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        if (encounter == null)
        {
            Debug.LogError("Debes asignar un enfrentamiento a la sala de combate!");
            return;
        }
        if (FloorEnd)
        {
            GameManager.Instance.OnBossFight = true;
        }

        room.MarkAsCurrent();
        room.MarkAsCleared();
        sceneChanger.LoadCombatScene(encounter);
    }
}
