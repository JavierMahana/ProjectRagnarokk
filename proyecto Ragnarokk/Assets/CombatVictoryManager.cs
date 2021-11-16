using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatVictoryManager : MonoBehaviour
{
    private ExperiencePanelManager expManager;
    private EvolutionPanelManager evolutionManager;

    private InfoBox infoBox;

    private CombatEncounter encounter;

    private void Start()
    {
        infoBox = FindObjectOfType<InfoBox>(true);
        if (infoBox == null)
            Debug.LogError("You need a info box!");

        expManager = FindObjectOfType<ExperiencePanelManager>(true);
        if (expManager == null)
            Debug.LogError("You need a info box!");

        evolutionManager = FindObjectOfType<EvolutionPanelManager>(true);
        if (evolutionManager == null)
            Debug.LogError("You need a info box!");


        encounter = GameManager.Instance.currentEncounter;

        StartCoroutine(ShowScene());
        
    }

    IEnumerator ShowScene()
    {
        infoBox.ShowInfo("Victory!", $"You eraned {encounter.MoneyReward}$ and {encounter.ExpReward} EXP!");


        yield return StartCoroutine(expManager.Init(encounter.ExpReward));

        //SceneChanger.Instance.LoadExplorationScene();
    }

}
