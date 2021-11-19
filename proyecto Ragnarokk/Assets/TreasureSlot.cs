using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider), typeof(SpriteRenderer))]
public class TreasureSlot : MonoBehaviour
{
    [HideInInspector]
    public Item CurrItem;

    private TreasureRoomManager manager;
    private InfoBox infoBox;
    private ConfirmScreen confirmScreen;
    private WeaponSwapManager weaponSwapManager;
    private void Start()
    {
        weaponSwapManager = FindObjectOfType<WeaponSwapManager>(true);
        if (weaponSwapManager == null)
            Debug.LogError("You need a weapon Swap Manager!");

        confirmScreen = FindObjectOfType<ConfirmScreen>(true);
        if (confirmScreen == null)
            Debug.LogError("You need a confirm screen!");

        infoBox = FindObjectOfType<InfoBox>(true);
        if (infoBox == null)
            Debug.LogError("You need a info box!");
    }

    public void Init(Item item, TreasureRoomManager manager)
    {
        CurrItem = item;
        this.manager = manager;     

        var image = GetComponent<Image>();
        image.sprite = item.IconSprite;
        image.preserveAspect = true;

        var renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = item.IconSprite;

    }

    void OnMouseDown() 
    {
        var itemAsWeapon = CurrItem as Weapon;
        var itemAsConsumible = CurrItem as Consumible;
        if (itemAsWeapon != null)
        {
            weaponSwapManager.Show(itemAsWeapon);
            //show weapon swap menu.
        }
        else if (itemAsConsumible != null)
        {
            confirmScreen.Show(itemAsConsumible);

        }

        //Debug.Log($"Click on {CurrItem.Name}");
    }

    void OnMouseEnter()
    {
        if (CurrItem != null)
        {
            infoBox.ShowInfo(CurrItem);
            //manager.ShowItemInfoBox(CurrItem);
        }
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
