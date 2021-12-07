using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class WeaponsPanel : MonoBehaviour
{
    public Image thisImage;
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI ValueDamage;
    public TextMeshProUGUI ValueAccuracy;
    public TextMeshProUGUI ValueCriticalChance;
    public TextMeshProUGUI ValueCooldown;
    public TextMeshProUGUI ValueType;

    public TextMeshProUGUI name1;
    public TextMeshProUGUI name2;
    public TextMeshProUGUI name3;

    public WeaponMenuButton[] TeamWeapons;
    // slot [12] guarda una copia de la última arma escogida
    private WeaponMenuButton Recall;
    public WeaponMenuButton currentWeaponButton;

    public bool OnChange;

    public Button exchange;
    public Button drop;
    public Button cancel;

    public GameObject OptionPanel;
    public void SetUpPanel()
    {
        //fills the weapons
        OnChange = false;
        FillWeaponsPanel();
    }
    public void Update()
    {
        var GM = FindObjectOfType<GeneralMenu>();
        if(GM.MenuDropdown.value == 2)
        {
            //Updates Icon and color of all buttons
            UpdateAllbuttons();

            if (currentWeaponButton == null)
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

    }

    public void FillWeaponsPanel()
    {
        EmptyArray();
        name1.text = GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>().RealName;
        name2.text = GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>().RealName;
        name3.text = GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>().RealName;

        for (int i = 0; i < 4; i++)
        {
            
            TeamWeapons[i].FillWeaponButton(GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>(), i);
            TeamWeapons[i+4].FillWeaponButton(GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>(), i);
            TeamWeapons[i+8].FillWeaponButton(GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>(), i);
        }
    }

    public void EmptyArray()
    {
        for (int i = 0; i < 12; i++)
        {
            TeamWeapons[i].thisWeapon = null;
            TeamWeapons[i].thisWeaponName.text = "";
            TeamWeapons[i].thisImage.sprite = null;
        }
    }

    public void TeamWeaponsUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>().Weapons[i] = TeamWeapons[i].thisWeapon;
            GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>().Weapons[i] = TeamWeapons[i+4].thisWeapon;
            GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>().Weapons[i] = TeamWeapons[i+8].thisWeapon;
        }
    }

    public void UpdateAllbuttons()
    {
        
        for (int i = 0; i < 12; i++)
        {
            TeamWeapons[i].UpdateButton();

            //updates currentWeapon the picked color
            /*
            if (currentWeaponButton == TeamWeapons[i])
            {
                if(currentWeaponButton.thisWeapon != null)
                {
                    TeamWeapons[i].thisImage.color = currentWeaponButton.GetComponent<Button>().colors.pressedColor;
                    if (OnChange)
                    {
                        currentWeaponButton.GetComponent<Image>().color = exchange.GetComponent<Button>().colors.selectedColor;
                    }
                    else
                    {
                        currentWeaponButton.GetComponent<Image>().color = Color.white;
                    }
                }
                else
                {
                    TeamWeapons[i].thisImage.color = Color.clear;
                }
                
            } */
        }
        
    }

    public void UpdateWeaponPanel(WeaponMenuButton button)
    {
        if (button != null && button.thisWeapon != null)
        {
            thisImage.sprite = button.thisImage.sprite;
            thisImage.preserveAspect = true;
            thisImage.color = button.defaultColor;
            Description.text = button.thisWeapon.Description;
            WeaponName.text = button.thisWeapon.Name;
            ValueDamage.text = button.thisWeapon.BaseDamage.ToString();
            ValueAccuracy.text = button.thisWeapon.BaseAccuracy.ToString();
            ValueCriticalChance.text = button.thisWeapon.BaseCriticalRate.ToString(); 
            ValueCooldown.text = button.thisWeapon.BaseCooldown.ToString();
            ValueType.text = button.thisWeapon.TipoDeDañoQueAplica.Name.ToString();
        }
        else
        {
            EmptyWeaponPanel();
        }
    }

    public void EmptyWeaponPanel()
    {
        thisImage.sprite = null;
        thisImage.color = Color.clear;
        WeaponName.text = "No Weapon Selected";
        ValueDamage.text = "";
        ValueAccuracy.text = "";
        ValueCriticalChance.text = "";
        ValueCooldown.text = "";
        Description.text = "";
        ValueType.text = "";
    }
    public void PrintArray()
    {
        
         Debug.Log("//////////// ARMAS DEL ARRAY ///////////");
         for (int i = 0; i < 12; i++)
         {
             if(TeamWeapons[i].thisWeapon != null)
             Debug.Log($"(arma[{i}] = {TeamWeapons[i].thisWeapon.Name}");
         }
         Debug.Log("////////////////////////////");
        /*
       Debug.Log("//////////// ARMAS DE LOS FIGHTERS ///////////");
        Debug.Log("Fighter1");
        for (int i = 0; i < 4; i++)
        {
            if(GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>().Weapons[i] != null)
            {
                Debug.Log(GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>().Weapons[i].Name);
            }

            else
            {
                Debug.Log("Empty");
            }
        }
        Debug.Log("Fighter2");
        for (int i = 0; i < 4; i++)
        {
            if(GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>().Weapons[i] != null)
            {
                Debug.Log(GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>().Weapons[i].Name);
            }
            else
            {
                Debug.Log("Empty");
            }

        }
        Debug.Log("Fighter3");
        for (int i = 0; i < 4; i++)
        {
            if(GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>().Weapons[i] != null)
            {
                Debug.Log(GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>().Weapons[i].Name);
            }

            else
            {
                Debug.Log("Empty");
            }
        }
        Debug.Log("////////////////////////////");
      */
    }
    // se activa al hacer clic al boton de un arma

    public void OnWeaponClick(WeaponMenuButton button)
    {
        
        if (currentWeaponButton != null)
        {
            Recall = button; 
        }

        if (OnChange)
        {
            var d = button.thisWeapon;
            button.thisWeapon = currentWeaponButton.thisWeapon;
            currentWeaponButton.thisWeapon = d;

            TeamWeaponsUpdate();

            FillWeaponsPanel();
            SetChange();

            currentWeaponButton = null;

            var AM = FindObjectOfType<AudioManager>();
            AM.Play("WeaponExchange");


            EmptyWeaponPanel();
            UpdateAllbuttons();
        }
        else
        {
            currentWeaponButton = button;
            UpdateWeaponPanel(button);
        }

    }

    // se activa al hacer clic al boton drop del OptionPanel
    public void DropWeapon()
    {
        if(currentWeaponButton != null)
        {
            for(int i = 0; i < 12; i++)
            {
                if(currentWeaponButton == TeamWeapons[i])
                {
                    currentWeaponButton.thisWeapon = null;
                }
            }
            TeamWeaponsUpdate();
                        
        }
        else
        {
            Debug.Log("no valid weapon selected");
        }

        currentWeaponButton = null;
        FillWeaponsPanel();
    }

    public void SetChange()
    {
        if (!OnChange)
        {
            OnChange = true;
            //exchange.GetComponent<Image>().color = exchange.GetComponent<Button>().colors.selectedColor;
        }
        else 
        { 
            OnChange = false;         
            //exchange.GetComponent<Image>().color = Color.white;
        }
    }

    public void Cancel()
    {
        currentWeaponButton = null;
        SetChange();
    }

}
