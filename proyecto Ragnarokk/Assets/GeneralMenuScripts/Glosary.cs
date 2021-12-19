using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Glosary : MonoBehaviour
{
    private List<GameObject> SynergyButtons = new List<GameObject>();
    private List<GameObject> AntiSynergyButtons = new List<GameObject>();

    public List<CombatType> DamageTypes;

    public GameObject PrefabDamageTypeButton;

    public GameObject PanelSynergies;
    public GameObject PanelAntiSynergies;

    public TextMeshProUGUI nombreDelEstado; 

    void Start()
    {
        CleanButtons();
        nombreDelEstado.text = "Ningún estado seleccionado";
    }

    public void CleanButtons()
    {
        foreach(GameObject o in SynergyButtons) { Destroy(o); }
        foreach (GameObject o in AntiSynergyButtons) { Destroy(o); }
        SynergyButtons.Clear();
        AntiSynergyButtons.Clear();
    }

    public void FillPanels(Button stateButton)
    {
        var state = stateButton.GetComponent<StateButton>().thisState;
        CleanButtons();

        if (nombreDelEstado.text != state.Name)
        {
            nombreDelEstado.text = state.Name;

            foreach (CombatType type in DamageTypes)
            {
                if (type.Sinergias.Contains(state))
                {
                    GameObject button = Instantiate(PrefabDamageTypeButton);
                    button.GetComponent<Button_DamageType>().SetButton(type);

                    button.transform.SetParent(PanelSynergies.transform, false);
                    SynergyButtons.Add(button);
                }

                if (type.AntiSinergias.Contains(state))
                {
                    GameObject button = Instantiate(PrefabDamageTypeButton);
                    button.GetComponent<Button_DamageType>().SetButton(type);

                    button.transform.SetParent(PanelAntiSynergies.transform, false);
                    AntiSynergyButtons.Add(button);
                }
            }
        }
        else
        {
            nombreDelEstado.text = "Ningún estado seleccionado";
        }
           
    }

}
