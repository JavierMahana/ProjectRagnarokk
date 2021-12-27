using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapManager : MonoBehaviour
{
    public GameObject content;

    public WeaponPanel[] fighterWeaponPanels = new WeaponPanel[12];
    public WeaponPanel NewWeaponPanel;

    
    public string defaultInfoBoxTitle = "Selecciona el arma que quieres intercambiar";
    public string defaultInfoBoxDescription = "¡El arma que escojas se perderá por siempre!\nEscoge con cuidado...";

    public List<GameObject> ObjectsToHideWhenShow = new List<GameObject>();

    private bool isShop;

    public void Hide()
    {
        content.gameObject.SetActive(false);
        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(true);
        }
    }

    public void Show(Weapon newWeapon, int shopItemSlot = -1, bool isShop = false)
    {
        this.isShop = isShop;

        //este panel es solo para info. por eso no importa el fighter ni el slot.
        NewWeaponPanel.Init(newWeapon, this, null, -1, true, shopItemSlot,  this.isShop);
        UpdateContent(shopItemSlot, isShop);
        content.SetActive(true);

        foreach (var obj in ObjectsToHideWhenShow)
        {
            obj.SetActive(false);
        }
    }


    private void UpdateContent(int shopItemSlot, bool isShop)
    {
        var gameManager = GameManager.Instance;

        //se inicializan los paneles de los player fighters.
        int i = 0;
        foreach (var pfighter in gameManager.PlayerFighters)
        {
            var fighter = pfighter.GetComponent<Fighter>();
            for (int j = 0; j < fighter.Weapons.Length; j++)
            {
                var weapon = fighter.Weapons[j];
                if (isShop)
                {
                    fighterWeaponPanels[i].Init(weapon, this, fighter, j, false, shopItemSlot, true);
                }
                else
                {
                    fighterWeaponPanels[i].Init(weapon, this, fighter, j);
                }
                
                i++;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateContent(-1, false);
    }

}
