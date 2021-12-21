using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StateButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Color color;
    public CombatState thisState;
    public Image thisImage;
    void Start()
    {
        if(thisState!=null)
        {
            SetButton(thisState);
        }
    }

    public void SetButton(CombatState cs)
    {
        //var fab = GetComponent<ForAllButtons>().Normal = color;
        thisState = cs;
        thisImage.sprite = cs.Sprite;
        thisImage.preserveAspect = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var CM = FindObjectOfType<CombatManager>();
        if(CM!=null)
        {
            Debug.Log("CombatManager Found");
            CM.ClearPanelDescriptor();
            Debug.Log(thisState.name);
            string description = $"Estado: {thisState.name}";
            CM.SetlDescriptorText(description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var CM = FindObjectOfType<CombatManager>();
        if (CM != null)
        {
            CM.ClearPanelDescriptor();
        }
    }
}
