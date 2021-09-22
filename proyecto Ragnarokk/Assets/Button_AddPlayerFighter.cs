using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_AddPlayerFighter : MonoBehaviour
{
    public FighterData data;
    public GameObject playerObjetcPrefab;
    public GameManager gmManager;

    public void CreatePlayerFighter()
    {
        var obj = Instantiate(playerObjetcPrefab);
        gmManager.SetDataToFighterGO(obj, data, true);
        //gmManager.PlayerFighters.Add(obj)
    }
}
