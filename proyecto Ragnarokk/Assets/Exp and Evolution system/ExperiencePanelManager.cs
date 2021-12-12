using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePanelManager : MonoBehaviour
{
    public bool showingExtraEX = false;

    public GameObject content;

    public ExpPanel panel1;
    public ExpPanel panel2;
    public ExpPanel panel3;

    public float ExpAplySpeed = 10;


    private InfoBox infoBox;

    private EvolutionPanelManager evolutionManager;


    private void Start()
    {


        infoBox = FindObjectOfType<InfoBox>(true);
        if (infoBox == null)
            Debug.LogError("You need a info box!");
        evolutionManager = FindObjectOfType<EvolutionPanelManager>(true);
        if (evolutionManager == null)
            Debug.LogError("You need a info box!");

    }



    //aplica la experiencia en un periodo de tiempo.
    public IEnumerator ApplyExp(int expToApply, Fighter fighterToApply, float expApplySpeed)
    {
        bool areEvolution = fighterToApply.CurrentExp + expToApply >= fighterToApply.ExpNeededToLevelUp;

        int initialExp = fighterToApply.CurrentExp;
        int finalEXPAmmount; 
        if (areEvolution)
        {
            finalEXPAmmount = fighterToApply.ExpNeededToLevelUp;
        }
        else
        {
            finalEXPAmmount = fighterToApply.CurrentExp + expToApply;
        }
        

        float apliedExp = 0; 
        while (apliedExp < expToApply)
        {
            yield return null;
            apliedExp += Time.deltaTime * expApplySpeed;
            fighterToApply.CurrentExp = Mathf.RoundToInt(apliedExp) + initialExp;
        }

        fighterToApply.CurrentExp = finalEXPAmmount;

        //SE ACTIVA EL PANEL DE EVOLUCIÓN
        if (areEvolution)
        {
            content.SetActive(false);

            evolutionManager.Show(fighterToApply);
            yield return new WaitUntil(() => evolutionManager.showingEvolution == false);

            Debug.Log("Se termino la evolucion");
            content.SetActive(true);
        }
    }

    public IEnumerator Init(int totalEXP)
    {
        var fightersThatWillGainExp = GetFightersThatWillGainExp();
        if (fightersThatWillGainExp.Count == 0)
        {
            infoBox.ShowInfo("No fighter can recieve XP", "All your fighters are max level");
            yield return new WaitForSeconds(2);
            SceneChanger.Instance.LoadExplorationScene();
        }
        int expDivided = totalEXP / (fightersThatWillGainExp.Count + 1);

        Show(expDivided);

        //le da a cada uno su experiencia correspondiente.
        //acá puede suceder una subida de nivel!
        foreach (var fighter in fightersThatWillGainExp)
        {
            yield return StartCoroutine(ApplyExp(expDivided, fighter, ExpAplySpeed));
            panel1.UpdatePanel();
            panel2.UpdatePanel();
            panel3.UpdatePanel();
        }


        //se revisa denuevo si hay fighters que pueden recibir XP, ya que se les entrego XP anteriormente.
        fightersThatWillGainExp = GetFightersThatWillGainExp();
        if (fightersThatWillGainExp.Count == 0)
        {
            SceneChanger.Instance.LoadExplorationScene();
        }

        panel1.MarkAsReady();
        panel2.MarkAsReady();
        panel3.MarkAsReady();

        showingExtraEX = true;

        infoBox.ShowInfo("Give extra XP", $"Select a fighter to give {expDivided} extra XP");

    }

    public static List<Fighter> GetFightersThatWillGainExp()
    {
        var fightersThatWillGainExp = new List<Fighter>();
        foreach (var p in GameManager.Instance.PlayerFighters)
        {
            var pf = p.GetComponent<Fighter>();
            if (pf.IsMaxLevel || pf.CurrentHP <= 0)
            {
            }
            else
            {
                fightersThatWillGainExp.Add(pf);
            }
        }
        return fightersThatWillGainExp;
    }

    private void Show(int expDivided)
    {
        content.SetActive(true);

        int j = 0;
        //se muestran los paneles.
        foreach (var p in GameManager.Instance.PlayerFighters)
        {
            var pf = p.GetComponent<Fighter>();
            switch (j)
            {
                case 0:
                    panel1.Show(pf, expDivided, this);
                    break;
                case 1:
                    panel2.Show(pf, expDivided, this);
                    break;
                case 2:
                    panel3.Show(pf, expDivided, this);
                    break;
                default:
                    break;
            }
            j++;
        }


    }
}
