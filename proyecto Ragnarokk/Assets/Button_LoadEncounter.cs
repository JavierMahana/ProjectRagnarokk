using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button_LoadEncounter : MonoBehaviour
{
    private GameManager gmManager;
    public CombatEncounter encounterToLoad;

    private void Awake()
    {
        gmManager = FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        if (encounterToLoad == null)
        {
            Debug.LogError("Debes asignar la variable encounterToLoad");
            return;
        }

        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = encounterToLoad.name;
    }

    public void LoadEncounter()
    {

        Debug.Log("Loading scene");
        gmManager.LoadCombatScene(encounterToLoad);
    }
}
