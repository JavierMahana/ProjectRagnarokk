using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    

    public GameObject HealthBarPrevab;

    private List<HealthBar> activeHealthBars = new List<HealthBar>();

    private CombatManager combatManager;
    private bool initialized;

    Vector2 healthBarOffset = new Vector2(0, 1);

    private void Awake()
    {
        initialized = false;
        combatManager = FindObjectOfType<CombatManager>();
    }
    private void Update()
    {
        if (!initialized)
        {
            if (combatManager.initialized)
            {
                InitHealthBars();
            }
        }
    }

    public void InitHealthBars()
    {
        var allBars = FindObjectsOfType<HealthBar>(true);
        foreach (var bar in allBars)
        {
            Destroy(bar.gameObject);
        }

        foreach (var fighter in combatManager.AllCombatFighters)
        {
            var healthBarObj = Instantiate(HealthBarPrevab, fighter.transform);
            

            var rectTransform = (RectTransform)healthBarObj.transform;
            rectTransform.localPosition = new Vector3(healthBarOffset.x, healthBarOffset.y, 1);

            HealthBar healthBarComp;
            if (healthBarObj.TryGetComponent<HealthBar>(out healthBarComp))
            {
                activeHealthBars.Add(healthBarComp);
                healthBarComp.Init(fighter);
                
            }
            else
            {
                Debug.LogError("El Prefab de healthBar debe tener el componente HealthBar!");
                return;
            }
        }
        initialized = true;
    }
}
