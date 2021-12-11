using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Lore Room")]
public class LoreRoom : RoomData
{
    public string text = "";
    public Sprite sprite;

    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        room.TryActivateLorePanel(sprite, text);
        FindObjectOfType<GeneralMenu>().gameObject.SetActive(false);

        room.MarkAsCurrent();
        room.MarkAsCleared();
    }
}
