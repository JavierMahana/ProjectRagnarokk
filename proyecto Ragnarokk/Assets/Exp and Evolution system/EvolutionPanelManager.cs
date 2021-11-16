using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EvolutionPanelManager : MonoBehaviour
{
    [HideInInspector]
    public bool showingEvolution = false;

    public GameObject Root;

    public EvolutionPanel panel1;
    public EvolutionPanel panel2;

    public void Show(Fighter fighterEvolving)
    {
        showingEvolution = true;

        Root.SetActive(true);
        panel1.Show( fighterEvolving.Evolutions[0], fighterEvolving, this);
        panel2.Show(fighterEvolving.Evolutions[1], fighterEvolving, this);
    }
    public void Hide()
    {
        showingEvolution = false;
        Root.SetActive(false);
    }

}
