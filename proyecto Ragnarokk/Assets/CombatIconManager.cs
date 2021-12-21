using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatIconManager : MonoBehaviour
{
    public GameObject StateIconPrefab;
    List<GameObject> CurrentStateIcons = new List<GameObject>();

    public void UpdateStateIcons(List<Fighter> fighters)
    {
        /*
        //Destruir iconos de estado
        while(CurrentStateIcons.Count != 0)
        {
            GameObject icon = CurrentStateIcons[0];
            CurrentStateIcons.Remove(icon);
            Destroy(icon);
        }

        foreach (Fighter fighter in fighters)
        {
            for (float i = 0; i < fighter.States.Count; i++)
            {
                var stateIcon = Instantiate(StateIconPrefab, fighter.transform);

                var rectTransform = (RectTransform)stateIcon.transform;
                rectTransform.localPosition = new Vector3(i/2 + 1, 0, 1);

                stateIcon.GetComponent<SpriteRenderer>().sprite = fighter.States[(int)i].Sprite;
                CurrentStateIcons.Add(stateIcon);
            }
        }
        */
    }
}
