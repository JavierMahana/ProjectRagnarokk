using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Treasure Room")]
public class HealRoom : RoomData
{
    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        room.MarkAsCurrent();
        room.MarkAsCleared();
    }
}
