using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FloorToLoad
{
    TUTORIAL,
    FIRST,
    SECOND,
    VICTORY
}
public abstract class RoomData : ScriptableObject
{
    public bool FloorStart = false;
    public bool FloorEnd = false;
    public FloorToLoad floorToLoadAtFinish;

    public Sprite VisibleSprite;
    

    public abstract void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room);
}
