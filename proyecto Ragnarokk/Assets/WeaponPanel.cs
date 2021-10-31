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
    public void Init(Weapon weapon, WeaponSwapManager manager, Fighter fighter, int slot, bool onlyShowInfo = false)
    {
        this.fighter = fighter;
        this.slot = slot;
        curWeapon = weapon;
        swapManager = manager;
        this.onlyShowInfo = onlyShowInfo;

        if (curWeapon != null)
        {
            if(imageComp == null)
                imageComp = GetComponent<Image>();
            imageComp.sprite = weapon.sprite;
        }
    }

    private void Awake()
    {
        imageComp = GetComponent<Image>();
        imageComp.color = Color.white;
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

    private void OnMouseDown()
    {
        if (!onlyShowInfo)
        {
            //confirm UI
            confirmScreen.Show(swapManager.NewWeaponPanel.curWeapon, fighter, slot);
        }
    }

    private void OnMouseEnter()
    {


        if (curWeapon != null)
        {
            infoBox.ShowInfo(curWeapon);
        }
        else
        {
            infoBox.ShowInfo("Empty slot", "This slot doesn't have a weapon so it's safe to select!");
        }
    }
    private void OnMouseExit()
    {
        if (swapManager != null)
        {
            infoBox.ShowInfo(swapManager.defaultInfoBoxTitle, swapManager.defaultInfoBoxDescription);
        }
        else
        {
            infoBox.Clear();
        }
        
    }

    private bool mouse_over = false;
    void Update()
    {
        if (mouse_over && Input.GetMouseButtonDown(0))
        {
            OnMouseDown();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        //Debug.Log("Mouse enter");
        OnMouseEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        OnMouseExit();
        //Debug.Log("Mouse exit");
    }
}
