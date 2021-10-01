using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WeaponSlotManager : MonoBehaviour
{
    [Range(0, 2)]
    public int index;

    private Fighter fighter;
    public UI_WeaponSlot[] WeaponSlots = new UI_WeaponSlot[4];



    private void OnEnable()
    {
        var pfs = GameManager.Instance.PlayerFighters;
        if (pfs.Count != 3)
        {
            Debug.LogError("No puedes usar la UI de los slots de armas si es que no hay 3 peleadores del jugador");
            return;
        }

        var pf = pfs[index];
        fighter = pf.GetComponent<Fighter>();

        for (int i = 0; i < 4; i++)
        {
            var weapon = fighter.Weapons[i];
            var slot = WeaponSlots[i];

            slot.Init(weapon, fighter, i);
        }        
    }
}
