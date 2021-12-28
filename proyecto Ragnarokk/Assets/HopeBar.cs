using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HopeBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject HopePanel;
    public TextMeshProUGUI HopeAmmount;
    public TextMeshProUGUI HopeMultiplier;
    public Image faceEmoji;
    public Sprite[] faces;
    private Vector3 imagePos;
    private Vector3 anchor;
    private bool OnChange;


    void Start()
    {
        if (HopePanel.activeSelf) { HopePanel.SetActive(false); }        
        faceEmoji.preserveAspect = true;

        RectTransform myRectTransform = faceEmoji.GetComponent<RectTransform>();
        imagePos = myRectTransform.localPosition;
        anchor = myRectTransform.anchoredPosition;
        OnChange = false;
    }

    void Update()
    {
        var GM = GameManager.Instance;
        var HM = HopeManager.Instance;
        var CM = FindObjectOfType<CombatManager>();
        var EM = FindObjectOfType<ExplorationState>();

        byte hopePhase = HM.CurrentHopePhase;

        int value = Mathf.Abs(Mathf.CeilToInt((hopePhase - 1) / 2));
        faceEmoji.sprite = faces[value];

        if (HopePanel == null)
        {
            var hp = EM.Canvas.transform.Find("Panel");
            hp.transform.SetParent(transform, true);
        }

        if(GM.GameState == GAME_STATE.MENU)
        {
            HopePanel.transform.SetParent(EM.Canvas.transform, true);
            if(!OnChange)
            {
                
                OnChange = true;
                RectTransform myRectTransform = faceEmoji.GetComponent<RectTransform>();

                imagePos = myRectTransform.localPosition;
                myRectTransform.localPosition += new Vector3(0, 15, 0);
                
                //faceEmoji.transform.position = imagePos + new Vector3(0, 35, 0);
            }
            
        }
        else
        {
            HopePanel.transform.SetParent(transform, true);
        }

        if (GM.GameState == GAME_STATE.COMBAT || GM.GameState == GAME_STATE.EXPLORATION) 
        {
            if (OnChange) 
            {
                RectTransform myRectTransform = faceEmoji.GetComponent<RectTransform>();
                myRectTransform.localPosition = imagePos;
                //anchor = myRectTransform.anchoredPosition;

                OnChange = false;
            }
        }
        
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        var HM = HopeManager.Instance;
        

        string hAmmount = HM.PartyHope + " / " + HM.Limit;
        var hMult = HM.CombatFactor;
        hMult = (float)Math.Round(hMult, 1);
        Debug.Log("multiplicado: " + hMult);
        HopeAmmount.text = hAmmount.ToString();
        HopeMultiplier.text = "x" + hMult.ToString();

        HopePanel.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HopePanel.SetActive(false);
    }
}
