using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WeaponsPanel : MonoBehaviour
{
    public Image thisImage;
    public Text WeaponName;
    public Text Description;
    public Text ValueDamage;
    public Text ValueAccuracy;
    public Text ValueCriticalChance;
    public Text ValueCooldown;
    public Text ValueType;

    public Text name1;
    public Text name2;
    public Text name3;

    public WeaponMenuButton[] Fighter1Weapons;
    public WeaponMenuButton[] Fighter2Weapons;
    public WeaponMenuButton[] Fighter3Weapons;

    public WeaponMenuButton currentWeaponButton;
    public bool OnChange;

    public Button exchange;
    public Button drop;
    public Button cancel;

    public GameObject OptionPanel;
    void Start()
    {
        FillWeaponsPanel();
    }
    public void Update()
    {
        if(currentWeaponButton == null)
        {
            exchange.interactable = false;
            drop.interactable = false;
            cancel.interactable = false;
        }
        else
        {
            exchange.interactable = true;
            drop.interactable = true;
            cancel.interactable = OnChange ? true : false;
        }
        
    }

    public void FillWeaponsPanel()
    {
        OnChange = false;

        var f1 = GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>();
        var f2 = GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>();
        var f3 = GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>();

        name1.text = f1.Name;
        name2.text = f2.Name;
        name3.text = f3.Name;

        for (int i = 0; i < 4; i++)
        {
            Fighter1Weapons[i].FillWeaponButton(f1, i);
            Fighter2Weapons[i].FillWeaponButton(f2, i);
            Fighter3Weapons[i].FillWeaponButton(f3, i);
        }
        UpdateWeaponPanel();
    }

    public void UpdateWeaponPanel()
    {
        if (currentWeaponButton != null && currentWeaponButton.thisWeapon != null)
        {
            thisImage.sprite = currentWeaponButton.thisImage.sprite;
            Description.text = currentWeaponButton.thisWeapon.Description;
            WeaponName.text = currentWeaponButton.thisWeapon.Name;
            ValueDamage.text = currentWeaponButton.thisWeapon.BaseDamage.ToString();
            ValueAccuracy.text = currentWeaponButton.thisWeapon.BaseAccuracy.ToString();
            //ValueCriticalChance.text = ;
            ValueCooldown.text = currentWeaponButton.thisWeapon.BaseCooldown.ToString();
            ValueType.text = currentWeaponButton.thisWeapon.TipoDeDañoQueAplica.ToString();
        }
        else
        {
            thisImage.sprite = null;
            WeaponName.text = "";
            ValueDamage.text = "";
            ValueAccuracy.text = "";
            ValueCriticalChance.text = "";
            ValueCooldown.text = "";
            Description.text = "";
            ValueType.text = "";

        }
    }

    // se activa al hacer clic al boton de un arma
    public void OnWeaponClick(WeaponMenuButton button)
    {
        if (OnChange)
        {
            SetChange();

            WeaponMenuButton place2 = button;
            WeaponMenuButton place1 = currentWeaponButton;

            #region Exchange
            
            for (int i = 0; i < 4; i++)
            {
                if (place2 == Fighter1Weapons[i]) 
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (place1 == Fighter1Weapons[j])
                        {
                            Fighter1Weapons[j] = place2;
                            Fighter1Weapons[i] = place1;
                        }
                        if (place1 == Fighter2Weapons[j])
                        {
                            Fighter1Weapons[j] = place2;
                            Fighter2Weapons[i] = place1;   
                        }
                        if (place1 == Fighter3Weapons[j])
                        {
                            Fighter1Weapons[j] = place2;
                            Fighter3Weapons[i] = place1;
                        }
                    }
                }
                 if (place2 == Fighter2Weapons[i]) 
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (place1 == Fighter1Weapons[j])
                        {
                            Fighter2Weapons[j] = place2;
                            Fighter1Weapons[i] = place1;
                        }
                        if(place1 == Fighter2Weapons[j])
                        {
                            Fighter2Weapons[j] = place2;
                            Fighter2Weapons[i] = place1;
                        }
                        if(place1 == Fighter3Weapons[j])
                        {
                            Fighter2Weapons[j] = place2;
                            Fighter3Weapons[i] = place1;
                        }
                    }
                }
               if (place2 == Fighter3Weapons[i]) 
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (place1 == Fighter1Weapons[j])
                        {
                            Fighter3Weapons[j] = place2;
                            Fighter1Weapons[i] = place1;
                        }
                        if(place1 == Fighter2Weapons[j])
                        {
                            Fighter3Weapons[j] = place2;
                            Fighter2Weapons[i] = place1;
                        }
                        if(place1 == Fighter3Weapons[j])
                        {
                            Fighter3Weapons[j] = place2;
                            Fighter3Weapons[i] = place1; 
                        }
                    }
                }
            }
            
            #endregion
   
            currentWeaponButton = null;
            FillWeaponsPanel();
        }
        else
        {
            if(button.thisWeapon != null)
            {
                if(currentWeaponButton != null)
                {
                    currentWeaponButton.UpdateButton();
                }
                currentWeaponButton = button;
                button.thisImage.color = Color.red;
            }
            
        }

        UpdateWeaponPanel();
    }

    // se activa al hacer clic al boton drop del OptionPanel
    public void DropWeapon()
    {
        if(currentWeaponButton != null)
        {
            foreach (PlayerFighter pf in GameManager.Instance.PlayerFighters)
            {
                var f = pf.GetComponent<Fighter>();
                for (int i = 0; i < 4; i++)
                {
                    if (f.Weapons[i] == currentWeaponButton.thisWeapon)
                    {
                        currentWeaponButton.thisWeapon = null;
                        f.Weapons[i] = null;
                    }
                }
            }            
        }
        else
        {
            Debug.Log("no valid weapon selected");
        }

        currentWeaponButton.UpdateButton();
        currentWeaponButton = null;
        FillWeaponsPanel();
    }

    public void SetChange()
    {
        if (!OnChange)
        {
            OnChange = true;
            exchange.GetComponent<Image>().color = Color.red;
        }
        else 
        { 
            OnChange = false;
            exchange.GetComponent<Image>().color = Color.white;
        }
    }

    public void Cancel()
    {
        currentWeaponButton.thisImage.color = Color.white;
        currentWeaponButton = null;
        SetChange();
        UpdateWeaponPanel();

    }

}
