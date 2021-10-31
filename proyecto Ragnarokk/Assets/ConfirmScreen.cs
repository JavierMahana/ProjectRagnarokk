using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmScreen : MonoBehaviour
{
    public List<GameObject> ObjectsToHideWhenShow = new List<GameObject>();
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

            Debug.Log("Se agrega el consumible al inventario. Eso aun no se implementa :o");

        }
        else if (currMoneyAmmount != -1) 
        {
            GameManager.Instance.CurrentMoney += currMoneyAmmount;
        }
        else
        {
            Debug.LogError("Can't confirm. The item selected is not a weapon nor a consumible.");
        }

        SceneChanger.Instance.LoadExplorationScene();
    }
    public void Cancel()
    {
        currItem = null;
        currFighter = null;
        currSlot = -1;
        currMoneyAmmount = -1;
        Hide();
    }

    public void Show(int moneyAmmount)
    {
        currMoneyAmmount = moneyAmmount;

        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }
    public void Show(Consumible consumible)
    {
        currItem = consumible;

        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }
    public void Show(Weapon weapon, Fighter fighter, int slot)
    {
        currItem = weapon;
        currFighter = fighter;
        currSlot = slot;

        Panel.SetActive(true);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }

    public void Hide()
    {
        Panel.SetActive(false);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(true);
        }
    }    
}
