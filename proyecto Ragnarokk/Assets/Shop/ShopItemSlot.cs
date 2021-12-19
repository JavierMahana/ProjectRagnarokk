using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Collider), typeof(SpriteRenderer))]
public class ShopItemSlot : MonoBehaviour
{
    public TextMeshProUGUI text;

    [HideInInspector]
    public Item CurrItem;

    private int shopItemSlot;

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
        if (CurrItem == null)
        {
            text.text = "";
        }

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


    public void Init(Item item, ShopManager manager, int shopItemSlot)
    {

        this.shopItemSlot = shopItemSlot;
        CurrItem = item;
        this.manager = manager;


        UpdateContent(item);
    }

    public void UpdateContent(Item item)
    {

        var renderer = GetComponent<SpriteRenderer>();
        

        if (item != null)
        {
            renderer.sprite = item.IconSprite;
            text.text = item.BaseCost.ToString();
        }
        else
        {
            CurrItem = null;
            renderer.sprite = manager.ItemSoldSprite;
            text.text = "";
        }        
    }


    void OnMouseDown()
    {
        if (CurrItem == null)
        {
            text.text = "";
            return;
        }

        if (GameManager.Instance.CurrentMoney < CurrItem.BaseCost)
        {
            text.text = "¡No tienes dinero suficiente!";
            showingAlternateText = true;
            return;
        }

        var itemAsWeapon = CurrItem as Weapon;
        var itemAsConsumible = CurrItem as Consumible;
        if (itemAsWeapon != null)
        {
            Debug.Log("Mostrando en shopitem");
            weaponSwapManager.Show(itemAsWeapon, shopItemSlot, true);
            //show weapon swap menu.
        }
        else if (itemAsConsumible != null)
        {
            confirmScreen.Show(itemAsConsumible, shopItemSlot, true);

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
