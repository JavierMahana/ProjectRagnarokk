using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Fighter target;
    public Image fillImage;  

    public void Init(Fighter target)
    {
        this.target = target;
        target.OnTakeDamage += HealthChanged;
    }
    public void ResetValues()
    {
        target.OnTakeDamage -= HealthChanged;
        target = null;
    }
    public void OnDisable()
    {
        ResetValues();
    }

    public void HealthChanged()
    {
        if (fillImage != null && target != null)
        {
            fillImage.fillAmount = target.CurrentHP <= 0 ? 0 : Mathf.Abs((float)target.CurrentHP / target.MaxHP);
        }
    }
}
