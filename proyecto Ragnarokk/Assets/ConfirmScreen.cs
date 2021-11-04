using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmScreen : MonoBehaviour
{
    public List<GameObject> ObjectsToHideWhenShow = new List<GameObject>();
    public GameObject WeaponsSwapObj;
    private bool showWeaponsSwapObjWhenClose = false;

    public GameObject Panel;
    
    private bool isShop;

    private Item currItem;

    private Fighter currFighter;
    private int currSlot;

    private int currMoneyAmmount;


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
        else
        {
            Debug.LogError("Can't confirm. The item selected is not a weapon nor a consumible.");
        }

        if (isShop)
        {
            Cancel();
        }
        else
        {
            SceneChanger.Instance.LoadExplorationScene();
        }
        
    }
    public void Cancel()
    {
        isShop = false;
        currItem = null;
        currFighter = null;
        currSlot = -1;
        currMoneyAmmount = -1;
        Hide();
    }

    public void Show(int moneyAmmount, bool isShop = false)
    {
        this.isShop = isShop;
        showWeaponsSwapObjWhenClose = false;

        currMoneyAmmount = moneyAmmount;

        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }
    public void Show(Consumible consumible, bool isShop = false)
    {
        this.isShop = isShop;
        showWeaponsSwapObjWhenClose = false;

        currItem = consumible;


        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }
    public void Show(Weapon weapon, Fighter fighter, int slot, bool isShop = false)
    {
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

    public void Hide()
    {
        Panel.SetActive(false);

        if (showWeaponsSwapObjWhenClose)
        {
            if (WeaponsSwapObj != null)
                WeaponsSwapObj.SetActive(true);
        }
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(true);
        }
    }    
}
