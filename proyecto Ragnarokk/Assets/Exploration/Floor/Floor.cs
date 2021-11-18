using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Exploration/Floor")]
public class Floor : SerializedScriptableObject
{
    public Sprite NotVisibleSprite;
    public RoomData[,] RoomLayout = new RoomData[10,10];

}
