using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Fighter target;
    public Image fillImage;

    public void Init(Fighter target)
    {
        this.target = target;
    }


    private void Update()
    {
        if (target == null)
        {
            return;
        }


        if (target.ExpNeededToLevelUp == 0)
        {
            fillImage.fillAmount = 0;
            return;
        }
        if (target.CurrentExp > target.ExpNeededToLevelUp)
        {
            target.CurrentExp = target.ExpNeededToLevelUp;
        }

        fillImage.fillAmount = Mathf.Abs((float)target.CurrentExp / target.ExpNeededToLevelUp);
    }
}
