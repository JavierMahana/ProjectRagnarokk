using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Lore Room")]
public class LoreRoom : RoomData
{

    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        room.MarkAsCurrent();
        room.MarkAsCleared();
    }
}
