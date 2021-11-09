using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Button_AddPlayerFighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public FighterData data;
    public GameObject playerObjetcPrefab;

    private InfoBox infoBox;
    private bool mouse_over = false;

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

        infoBox = FindObjectOfType<InfoBox>(true);
        if (infoBox == null)
            Debug.LogError("You need a info box!");
    }

    public void CreatePlayerFighter()
    {
        var pfs = FindObjectsOfType<PlayerFighter>();
        int pfsCount = pfs.Length;

        var obj = Instantiate(playerObjetcPrefab);
        GameManager.Instance.SetDataToFighterGO(obj, data, true);

        obj.transform.position = new Vector2( (-1 + pfsCount),1);
    }


    private void OnDisable()
    {
        infoBox.Clear();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        if (data != null)
        {
            infoBox.ShowInfo(data);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        infoBox.ShowInfo("Choose your team","");
    }
}
