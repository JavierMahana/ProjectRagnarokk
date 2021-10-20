using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_AddPlayerFighter : MonoBehaviour
{
    public FighterData data;
    public GameObject playerObjetcPrefab;

    private void Start()
    {
        if (data != null)
        {
            Text text = GetComponentInChildren<Text>();
            if (text != null)
            {
                text.text = data.Name;
            }
        }
    }

    public void CreatePlayerFighter()
    {
        var pfs = FindObjectsOfType<PlayerFighter>();
        int pfsCount = pfs.Length;

        var obj = Instantiate(playerObjetcPrefab);
        GameManager.Instance.SetDataToFighterGO(obj, data, true);

        obj.transform.position = new Vector2( (-1 + pfsCount),-3);
    }
}
