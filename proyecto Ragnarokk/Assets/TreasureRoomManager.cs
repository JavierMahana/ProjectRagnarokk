using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoomManager : MonoBehaviour
{
    //la idea es colocar los slots y la infobox manualmente desde el editor.
    public TreasureSlot[] treasureSlots = new TreasureSlot[3];
    public MoneySlot moneySlot;
    public InfoBox InfoBox;

    public string defaultInfoBoxTitle = "Sala del Tesoro";
    public string defaultInfoBoxDescription = "¡Solo puedes escoger 1 tesoro!\nEscoge cuidadosamente...";

    void Start()
    {
        if (InfoBox == null)
        {
            InfoBox = FindObjectOfType<InfoBox>(true);
            if (InfoBox == null)
                Debug.LogError("You need a info box!");
        }

        moneySlot.Init(GameManager.Instance.treasureRoomMoney, this);

        for (int i = 0; i < treasureSlots.Length; i++)
        {
            var currSlot = treasureSlots[i];
            currSlot.Init(GameManager.Instance.currentTreasureItems[i], this);
        }

        if (InfoBox == null)
        {
            Debug.LogError("You need a infobox!");
        }
        InfoBox.ShowInfo(defaultInfoBoxTitle, defaultInfoBoxDescription);
        GameManager.Instance.CheckOnLoadScene();

    }

    //public void ShowItemInfoBox(Item item)
    //{
    //    if (InfoBox == null)
    //    {
    //        Debug.LogError("You need a infobox!");
    //    }
    //    InfoBox.ShowInfo(item);
    //}
    //public void ClearInfoBox()
    //{
    //    if (InfoBox == null)
    //    {
    //        Debug.LogError("You need a infobox!");
    //    }
    //    InfoBox.Hide();
    //}
}
