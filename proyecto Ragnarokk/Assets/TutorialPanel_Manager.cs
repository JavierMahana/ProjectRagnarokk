using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialPanel_Manager : MonoBehaviour
{
    //Agregar un sistema que vea si es la primera vez que se juega. para que la active automaticamente.
    enum ESTADO_TUTORIAL
    {
        SELECT,
        EXPLORATION,
        COMBAT,
        HIDEN
    }




    private ESTADO_TUTORIAL estadoActual;
    private int indicePanelActual;

    public List<GameObject> CombatObjectAdds = new List<GameObject>();

    public List<Sprite> CombatSreenshotImages = new List<Sprite>();
    private int combatPanelCount => CombatSreenshotImages.Count;


    public List<GameObject> ExplorationObjectAdds = new List<GameObject>();
    
    public List<Sprite> ExplorationSreenshotImages = new List<Sprite>();
    private int explorationPanelCount => ExplorationSreenshotImages.Count;

    public GameObject Background;
    public GameObject SelectParentObjetct;
    public GameObject TutorialParentObjetct;

    public TextMeshProUGUI TextoBottonContinuar;
    public Image MainImage;


    public void Hide()
    {
        estadoActual = ESTADO_TUTORIAL.HIDEN;
        indicePanelActual = 0;

        Background.SetActive(false);
        SelectParentObjetct.SetActive(false);
        TutorialParentObjetct.SetActive(false);
    }
    public void ShowExplorationTutorial()
    {
        CambiarDeEstado(ESTADO_TUTORIAL.EXPLORATION);
    }
    public void ShowCombatTutorial()
    {
        CambiarDeEstado(ESTADO_TUTORIAL.COMBAT);
    }

    public void SiguienteButton()
    {
        indicePanelActual++;

        int panelCount = estadoActual == ESTADO_TUTORIAL.EXPLORATION ? explorationPanelCount : combatPanelCount;

        if (indicePanelActual == panelCount)
        {
            if (estadoActual == ESTADO_TUTORIAL.EXPLORATION && GameManager.Instance.SeDebeMostrarElTutorial())
            {
                CambiarDeEstado(ESTADO_TUTORIAL.COMBAT);
            }
            else
            {
                if (GameManager.Instance.SeDebeMostrarElTutorial())
                {
                    GameManager.Instance.TutorialComplete();
                    CambiarDeEstado(ESTADO_TUTORIAL.HIDEN);
                }
                else
                {
                    CambiarDeEstado(ESTADO_TUTORIAL.SELECT);
                }


            }
        }
        else
        {
            
            MostrarPanelTutorial();
        }

    }

    public void IniciarTutorial()
    {
        CambiarDeEstado(ESTADO_TUTORIAL.SELECT);
    }


    void Start()
    {
        PlayerPrefs.SetInt("tutorialComplete", 0);

        if (GameManager.Instance.SeDebeMostrarElTutorial())
        {
            CambiarDeEstado(ESTADO_TUTORIAL.EXPLORATION);
        }
        else
        {
            CambiarDeEstado(ESTADO_TUTORIAL.HIDEN);
            
        }
    }





    private void CambiarDeEstado(ESTADO_TUTORIAL estadoNuevo)
    {
        estadoActual = estadoNuevo;
        indicePanelActual = 0;

        switch (estadoNuevo)
        {
            case ESTADO_TUTORIAL.SELECT:
                MostrarPanelSelect();
                break;
            case ESTADO_TUTORIAL.EXPLORATION:
                MostrarPanelTutorial();
                break;
            case ESTADO_TUTORIAL.COMBAT:
                MostrarPanelTutorial();
                break;
            case ESTADO_TUTORIAL.HIDEN:
                Hide();
                break;
            default:
                Debug.Log("el estado del tutorial que se quiere ir no existe.");
                break;
        }
    }



    private void MostrarPanelSelect()
    {
        Background.SetActive(true);
        SelectParentObjetct.SetActive(true);
        TutorialParentObjetct.SetActive(false);
    }

    private void MostrarPanelTutorial()
    {
        Background.SetActive(true);
        SelectParentObjetct.SetActive(false);
        TutorialParentObjetct.SetActive(true);

        foreach (var obj in CombatObjectAdds)
        {
            obj.SetActive(false);
        }
        foreach (var obj in ExplorationObjectAdds)
        {
            obj.SetActive(false);
        }


        Sprite spriteACargar = null;
        GameObject objetoPadreExtrasACargar = null;

        if (estadoActual == ESTADO_TUTORIAL.COMBAT)
        {
            if (indicePanelActual - 1 == combatPanelCount)
                TextoBottonContinuar.text = "Salir";
            else
                TextoBottonContinuar.text = "Siguiente";

            spriteACargar = CombatSreenshotImages[indicePanelActual];
            objetoPadreExtrasACargar = CombatObjectAdds[indicePanelActual];

        }
        else if (estadoActual == ESTADO_TUTORIAL.EXPLORATION)
        {
            if (indicePanelActual - 1 == explorationPanelCount && !GameManager.Instance.SeDebeMostrarElTutorial())
                TextoBottonContinuar.text = "Salir";
            else
                TextoBottonContinuar.text = "Siguiente";

            spriteACargar = ExplorationSreenshotImages[indicePanelActual];
            objetoPadreExtrasACargar = ExplorationObjectAdds[indicePanelActual];
        }
        else
        {
            Debug.LogWarning("No es posible cambiar de panel");
            return;
        }

        MainImage.sprite = spriteACargar;
        objetoPadreExtrasACargar.SetActive(true);


        
    }



}
