using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Lore Room")]
public class LoreRoom : RoomData
{
    private string text = "";
    public Sprite sprite;

    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        text = ExplorationManager.Instance.LoreManager.GetNewLore();

        room.TryActivateLorePanel(sprite, text);
        FindObjectOfType<GeneralMenu>().gameObject.SetActive(false);

        room.MarkAsCurrent();
        room.MarkAsCleared();
    }

    List<string> lore = new List<string>();

}
