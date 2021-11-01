using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(SpriteRenderer))]
public class MoneySlot : MonoBehaviour
{
    [HideInInspector]
    private int moneyAmmount = 0;

    private TreasureRoomManager manager;
    private InfoBox infoBox;
    private ConfirmScreen confirmScreen;
    private void Start()
    {
        confirmScreen = FindObjectOfType<ConfirmScreen>(true);
        if (confirmScreen == null)
            Debug.LogError("You need a confirm screen!");

        infoBox = FindObjectOfType<InfoBox>(true);
        if (infoBox == null)
            Debug.LogError("You need a info box!");
    }

    public void Init(int moneyAmmount, TreasureRoomManager manager)
    {
        this.moneyAmmount = moneyAmmount;
        this.manager = manager;
    }

    void OnMouseDown()
    {
        confirmScreen.Show(moneyAmmount);
    }

    void OnMouseEnter()
    {
        infoBox.ShowInfo("Money", $"you can grab {moneyAmmount} coins instead of a weapon.");
    }
    void OnMouseExit()
    {
        if (manager != null)
        {
            infoBox.ShowInfo(manager.defaultInfoBoxTitle, manager.defaultInfoBoxDescription);
        }
        else
        {
            infoBox.Clear();
        }
    }
}
