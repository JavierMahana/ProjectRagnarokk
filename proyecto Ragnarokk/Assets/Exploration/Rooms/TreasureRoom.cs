using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Treasure Room")]
public class TreasureRoom : RoomData
{
    List<Weapon> AllPosibleTreasures = new List<Weapon>();

    public override void LoadRoom(SceneChanger sceneChanger)
    {
        throw new System.NotImplementedException();
    }
}
