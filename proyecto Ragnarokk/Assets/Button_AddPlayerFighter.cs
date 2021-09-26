using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_AddPlayerFighter : MonoBehaviour
{
    public FighterData data;
    public GameObject playerObjetcPrefab;

    public void CreatePlayerFighter()
    {
        var obj = Instantiate(playerObjetcPrefab);
        GameManager.Instance.SetDataToFighterGO(obj, data, true);
    }
}
