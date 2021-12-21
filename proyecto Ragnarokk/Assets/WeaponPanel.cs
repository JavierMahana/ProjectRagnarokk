using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WeaponPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image imageComp;


    private Weapon curWeapon;
    private Fighter fighter;
    private int slot;

    private InfoBox infoBox;
    private ConfirmScreen confirmScreen;

    private WeaponSwapManager swapManager;
    private bool onlyShowInfo = false;

    private bool isShop = false;

    private int shopItemSlot;

    public void Init(Weapon weapon, WeaponSwapManager manager, Fighter fighter, int slot, bool onlyShowInfo = false, int shopItemSlot = -1, bool isShop = false)
    {
        this.shopItemSlot = shopItemSlot;
        this.isShop = isShop;

        this.fighter = fighter;
        this.slot = slot;
        curWeapon = weapon;
        swapManager = manager;
        this.onlyShowInfo = onlyShowInfo;

        if (curWeapon != null)
        {
            if(imageComp == null)
                imageComp = GetComponent<Image>();
            imageComp.sprite = weapon.IconSprite;
            imageComp.preserveAspect = true;
        }
    }

    private void Awake()
    {
        imageComp = GetComponent<Image>();
        //imageComp.color = Color.white;
    }
    private void Start()
    {
        confirmScreen = FindObjectOfType<ConfirmScreen>(true);
        if (confirmScreen == null)
            Debug.LogError("You need a confirm screen!");

        infoBox = FindObjectOfType<InfoBox>(true);
        if (infoBox == null)
            Debug.LogError("You need a info box!");
    }


    private bool frameShown = false;
    private bool mouse_over = false;
    void Update()
    {
        if (mouse_over && Input.GetMouseButtonDown(0) && !frameShown)
        {
            if (!onlyShowInfo)
            {
                //confirm UI
                Debug.Log($"mopstrando la confirm screen desde el weapon swap panel! es tienda? {isShop}");

                confirmScreen.Show(swapManager.NewWeaponPanel.curWeapon, fighter, slot, shopItemSlot, isShop);
            }
        }
        frameShown = false ;
    }

    private void OnEnable()
    {
        frameShown = true;
    }
    private void OnDisable()
    {
        mouse_over = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        if (curWeapon != null)
        {
            infoBox.ShowInfo(curWeapon);
        }
        else
        {
            infoBox.ShowInfo("Espacio Vac�o", "!Este espacio est� vac�o puedes utilizarlo sin problemas!");
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        if (swapManager != null)
        {
            infoBox.ShowInfo(swapManager.defaultInfoBoxTitle, swapManager.defaultInfoBoxDescription);
        }
        else
        {
            infoBox.Clear();
        }
    }
}
