using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Heal Room")]
public class HealRoom : RoomData
{
    public string text = "";
    public Sprite sprite;

    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        gameManager.HealPlayerFighters();
        room.TryActivateLorePanel(sprite, text);
        FindObjectOfType<GeneralMenu>().gameObject.SetActive(false);

        room.MarkAsCurrent();
        room.MarkAsCleared();
    }
}
