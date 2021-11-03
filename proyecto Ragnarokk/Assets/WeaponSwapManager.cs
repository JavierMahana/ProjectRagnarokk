using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapManager : MonoBehaviour
{
    public GameObject content;

    public WeaponPanel[] fighterWeaponPanels = new WeaponPanel[12];
    public WeaponPanel NewWeaponPanel;

    
    public string defaultInfoBoxTitle = "Pick a weapon to interchange";
    public string defaultInfoBoxDescription = "The weapon you choose will be droped forever!\nPick carefully...";


    private bool isShop = false;

    public void Show(Weapon newWeapon, bool isShop = false)
    {
        this.isShop = isShop;

        //este panel es solo para info. por eso no importa el fighter ni el slot.
        NewWeaponPanel.Init(newWeapon, this, null, -1, true, isShop);
        UpdateContent();
        content.SetActive(true);
    }


    private void UpdateContent()
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
                fighterWeaponPanels[i].Init(weapon, this, fighter, j);
                i++;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateContent();
    }

}
