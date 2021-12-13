using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmScreen : MonoBehaviour
{

    private ShopManager shopManager;

    private void Start()
    {
        shopManager = FindObjectOfType<ShopManager>();
    }

    public List<GameObject> ObjectsToHideWhenShow = new List<GameObject>();
    public GameObject WeaponsSwapObj;
    private bool showWeaponsSwapObjWhenClose = false;

    public GameObject Panel;
    
    private bool isShop;

    private Item currItem;

    private Fighter currFighter;
    private int currSlot;

    private int currMoneyAmmount;

    private int itemShopSlotIndex;

    private EvolutionPanelManager evolutionManager;
    private Fighter evolvingFighter;
    private FighterData evolvingData;

    public void Confirm()
    {
        Weapon itemAsWeapon = null;
        Consumible itemAsConsumible = null;
        if (currItem != null)
        {
            itemAsWeapon = currItem as Weapon;
            itemAsConsumible = currItem as Consumible;

            //si estamos en una tienda y se confirma la compra. se gasta el dinero que cuesta.
            if (isShop)
            {
                GameManager.Instance.CurrentMoney -= currItem.BaseCost;

                if (shopManager == null)
                {
                    Debug.LogError("To confirm the buy of an item you need a shopManager!");
                }
                ShopItemSlot slot = shopManager.ShopSlots[itemShopSlotIndex];
                slot.UpdateContent(null);
                GameManager.Instance.CurrShopData.UpdateContent(itemShopSlotIndex);
                    
            }
        }
        
        if (itemAsWeapon != null)
        {
            if (currFighter == null || currSlot == -1)
            {
                Debug.LogError("Can't confirm. The item selected is  weapon but there is not slot or fighter.");
                return;
            }
            currFighter.Weapons[currSlot] = itemAsWeapon;
        }
        else if (itemAsConsumible != null)
        {
            GameManager.Instance.AllConsumibles.Add(itemAsConsumible);

        }
        else if (currMoneyAmmount != -1) 
        {
            GameManager.Instance.CurrentMoney += currMoneyAmmount;
        }
        else if(evolvingFighter != null && evolvingData != null)
        {
            evolvingFighter.Init(evolvingData);
            evolvingFighter.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = evolvingData.Sprite;
            evolutionManager.Hide();
        }
        else
        {
            Debug.LogError("Can't confirm. The item selected is not a weapon nor a consumible.");
        }

        if (isShop)
        {
            Cancel(true);

        }
        else if (evolutionManager != null)
        {
            evolutionManager = null;
            evolvingData = null;
            evolvingFighter = null;
            isShop = false;
            currItem = null;
            currFighter = null;
            currSlot = -1;
            currMoneyAmmount = -1;
            Hide(false, true);            

        }
        else
        {
            SceneChanger.Instance.LoadExplorationScene();
        }

        FindObjectOfType<AudioManager>().Play("WeaponExchange");
        
    }
    public void Cancel(bool forceCloseSwapWeapon = false)
    {
        evolutionManager = null;
        evolvingData = null;
        evolvingFighter = null;
        isShop = false;
        currItem = null;
        currFighter = null;
        currSlot = -1;
        currMoneyAmmount = -1;


        Hide(forceCloseSwapWeapon);
    }

    public void Show(Fighter evolvingFighter, FighterData newData, EvolutionPanelManager evManager)
    {
        this.evolutionManager = evManager;
        this.evolvingFighter = evolvingFighter;
        this.evolvingData = newData;

        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {            
            obj.SetActive(false);
        }
    }
    public void Show(int moneyAmmount,int itemShopSlotIndex = -1,  bool isShop = false)
    {
        this.itemShopSlotIndex = itemShopSlotIndex;

        this.isShop = isShop;
        showWeaponsSwapObjWhenClose = false;

        currMoneyAmmount = moneyAmmount;

        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }
    public void Show(Consumible consumible,int itemShopSlotIndex = -1, bool isShop = false)
    {
        this.itemShopSlotIndex = itemShopSlotIndex;

        this.isShop = isShop;
        showWeaponsSwapObjWhenClose = false;

        currItem = consumible;


        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }
    public void Show(Weapon weapon, Fighter fighter, int slot, int itemShopSlotIndex = -1, bool isShop = false)
    {
        this.itemShopSlotIndex = itemShopSlotIndex;

        this.isShop = isShop;

        showWeaponsSwapObjWhenClose = true;

        currItem = weapon;
        currFighter = fighter;
        currSlot = slot;

        Panel.SetActive(true);

        if (WeaponsSwapObj != null)
            WeaponsSwapObj.SetActive(false);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }

    public void Hide(bool forceCloseSwapWeapon = false, bool ShowObjectsToHide = true)
    {
        Panel.SetActive(false);

        if (showWeaponsSwapObjWhenClose && !forceCloseSwapWeapon)
        {
            if (WeaponsSwapObj != null)
                WeaponsSwapObj.SetActive(true);
        }

        if (ShowObjectsToHide)
        {
            foreach (var obj in ObjectsToHideWhenShow)
            {
                obj.SetActive(true);
            }
        }
    }    
}
