using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Glosary : MonoBehaviour
{
    private List<GameObject> PositiveButtons = new List<GameObject>();
    private List<GameObject> NegativeButtons = new List<GameObject>();

    public List<CombatType> DamageTypes;

    public GameObject PrefabDamageTypeButton;
    public GameObject StateButtons;
    public GameObject DamageTypeButtons;

    public GameObject PanelPositive;
    public GameObject PanelNegative;

    private string defaultText = "Haz clic en los botones para ver los estados o los tipos de daño y sus respectivas fortalezas y debilidades.";
    public TextMeshProUGUI nombre;
    public TextMeshProUGUI positive;
    public TextMeshProUGUI negative;

    private bool thereIsSelected; 

    void Start()
    {
        Reset();        
    }

    public void Reset()
    {
        StateButtons.SetActive(false);
        DamageTypeButtons.SetActive(false);
        CleanButtons();
    }

    public void CleanButtons()
    {
        
        thereIsSelected = false;

        foreach (GameObject o in PositiveButtons) { Destroy(o); }
        foreach (GameObject o in NegativeButtons) { Destroy(o); }

        PositiveButtons.Clear();
        NegativeButtons.Clear();

        nombre.text = "Escoge un elemento";
        positive.text = "";
        negative.text = "";
    }

    public void Update()
    {
        if(!StateButtons.gameObject.activeInHierarchy && !DamageTypeButtons.gameObject.activeInHierarchy)
        {
            nombre.text = defaultText;
        }
        else if(StateButtons.gameObject.activeInHierarchy && !thereIsSelected)
        {
            nombre.text = "Estados";
        }
        else if(DamageTypeButtons.gameObject.activeInHierarchy && !thereIsSelected)
        {
            nombre.text = "Tipos de Daño";
        }

    }

    public void FillPanels(Button button)
    {
        
        CleanButtons();

        //Estados
        if(StateButtons.gameObject.activeInHierarchy)
        {
            thereIsSelected = true;
            positive.text = "Sinergias";
            negative.text = "Anti-Sinergias";

            var state = button.GetComponent<StateButton>().thisState;

            if (nombre.text != state.Name)
            {
                nombre.text = state.Name;

                foreach (CombatType type in DamageTypes)
                {
                    if (type.Sinergias.Contains(state))
                    {
                        GameObject newButton = Instantiate(PrefabDamageTypeButton);
                        newButton.GetComponent<Button_DamageType>().SetButton(type);

                        newButton.transform.SetParent(PanelPositive.transform, false);
                        PositiveButtons.Add(newButton);
                    }

                    if (type.AntiSinergias.Contains(state))
                    {
                        GameObject newButton = Instantiate(PrefabDamageTypeButton);
                        newButton.GetComponent<Button_DamageType>().SetButton(type);

                        newButton.transform.SetParent(PanelNegative.transform, false);
                        NegativeButtons.Add(newButton);
                    }
                }
            }
            else
            {
                nombre.text = "Ningún estado seleccionado";
            }
        }
        //tipos de daño
        else if (DamageTypeButtons.gameObject.activeInHierarchy)
        {
            thereIsSelected = true;
            positive.text = "Fuerte contra";
            negative.text = "Débil contra   ";

            var dmgType = button.GetComponent<Button_DamageType>().dmg;
            if (nombre.text != dmgType.name)
            {
                nombre.text = dmgType.name;

                foreach (CombatType type in DamageTypes)
                {
                    if (type.Debilidades.Contains(dmgType))
                    {
                        GameObject newButton = Instantiate(PrefabDamageTypeButton);
                        newButton.GetComponent<Button_DamageType>().SetButton(type);

                        newButton.transform.SetParent(PanelPositive.transform, false);
                        PositiveButtons.Add(newButton);
                    }

                    if (type.Resistencias.Contains(dmgType))
                    {
                        GameObject newButton = Instantiate(PrefabDamageTypeButton);
                        newButton.GetComponent<Button_DamageType>().SetButton(type);

                        newButton.transform.SetParent(PanelNegative.transform, false);
                        NegativeButtons.Add(newButton);
                    }
                }
            }
            else
            {
                nombre.text = "Ningún tipo de daño seleccionado";
                thereIsSelected = false;
            }
        }
    }

}
