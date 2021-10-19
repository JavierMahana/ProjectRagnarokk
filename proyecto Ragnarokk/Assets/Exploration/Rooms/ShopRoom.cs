using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Shop Room")]
public class ShopRoom : RoomData
{
    List<Weapon> AllPosibleItems = new List<Weapon>();

    public override void LoadRoom(SceneChanger sceneChanger)
    {
        throw new System.NotImplementedException();
    }
}
