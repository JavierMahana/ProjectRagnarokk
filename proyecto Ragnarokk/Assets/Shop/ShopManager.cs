using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public Sprite ItemSoldSprite;

    public string defaultInfoBoxTitle = "Tienda";
    public string defaultInfoBoxDescription = "Puedes comprar cualquier item.\nSi tienes suficiente dinero, claro...";


    public ShopItemSlot[] ShopSlots = new ShopItemSlot[6]; 

    void Start()
    {
        GameManager.Instance.ShowPlayerFighters(false);

        ShopSlots[0].Init(GameManager.Instance.CurrShopData.weapon1, this, 0);
        ShopSlots[1].Init(GameManager.Instance.CurrShopData.weapon2, this, 1);
        ShopSlots[2].Init(GameManager.Instance.CurrShopData.weapon3, this, 2);

        ShopSlots[3].Init(GameManager.Instance.CurrShopData.consumible1, this, 3);
        ShopSlots[4].Init(GameManager.Instance.CurrShopData.consumible2, this, 4);
        ShopSlots[5].Init(GameManager.Instance.CurrShopData.consumible3, this, 5);
    }

}
