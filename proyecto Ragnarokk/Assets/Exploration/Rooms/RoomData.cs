using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomData : ScriptableObject
{
    public bool FloorStart = false;
    public bool FloorEnd = false;

    public Sprite VisibleSprite;
    

    public abstract void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room);
}
