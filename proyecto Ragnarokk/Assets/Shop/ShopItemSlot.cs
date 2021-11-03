using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider), typeof(SpriteRenderer))]
public class ShopItemSlot : MonoBehaviour
{
    public TextMeshProUGUI text;

    [HideInInspector]
    public Item CurrItem;

    private int slot;

    private ShopManager manager;
    private InfoBox infoBox;
    private ConfirmScreen confirmScreen;
    private WeaponSwapManager weaponSwapManager;

    private float timeToShowText = 3f;
    private float currTimePassed = 0f;
    private bool showingAlternateText = false;

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

    private void Update()
    {
        if (showingAlternateText && CurrItem != null)
        {
            currTimePassed += Time.deltaTime;
            if (currTimePassed >= timeToShowText)
            {
                text.text = CurrItem.BaseCost.ToString();

                currTimePassed = 0;
                showingAlternateText = false;
            }
        }
    }


    public void Init(Item item, ShopManager manager, int slot)
    {
        this.slot = slot;

        CurrItem = item;
        this.manager = manager;

        var renderer = GetComponent<SpriteRenderer>();

        if (item != null)
        {
            renderer.sprite = item.sprite;
            text.text = item.BaseCost.ToString();
        }
        else
        {
            text.text = "";
        }
            

        
    }

    void OnMouseDown()
    {
        if (CurrItem == null)
        {
            return;
        }

        if (GameManager.Instance.CurrentMoney < CurrItem.BaseCost)
        {
            text.text = "Insuficient Founds!";
            showingAlternateText = true;
            return;
        }

        var itemAsWeapon = CurrItem as Weapon;
        var itemAsConsumible = CurrItem as Consumible;
        if (itemAsWeapon != null)
        {
            weaponSwapManager.Show(itemAsWeapon, true);
            //show weapon swap menu.
        }
        else if (itemAsConsumible != null)
        {
            confirmScreen.Show(itemAsConsumible, true);

        }
    }

    void OnMouseEnter()
    {
        if (CurrItem != null)
        {
            infoBox.ShowInfo(CurrItem);
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
