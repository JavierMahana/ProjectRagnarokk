using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HopeManager : Singleton<HopeManager>
{
    [HideInInspector]
    public bool Initialized;

    public GameObject CanvasHope;
    public GameObject HopeBar;
    public Image HopeBarFill;

    [Range(0, 100)]
    public float PartyHope; //La esperanza del grupo

    //La esperanza se trata como un porcentaje (esto podría cambiar a futuro)
    private const float MaxHope = 100;
    private const float MinHope = 0;

    private Dictionary<sbyte, float> HopeModifiers; //Relacionado a la magnitud con que se altera la esperanza

    //La esperanza consta de fases, que se van alcanzando cuando ésta se modifica.
    private const byte TotalHopePhases = 11; //Este valor se puede modificar a gusto, pero debe armonizar con BaseCombatFactor
    private byte CurrentHopePhase; //Las fases se cuentan de 0 a (TotalHopePhases - 1)
    private byte NeutralPhase; //Fase intermedia en la que el daño no se verá afectado por la esperanza

    private float PhaseInterval; //Distancia entre fases (en porcentaje)

    public const float BaseCombatFactor = 1.1f; //El multiplicador de daño se calcula como potencias de este valor
    public float CombatFactor;

    protected override void OnAwake()
    {
        ResetHope();

        //En este diccionario, un valor que será llamado "magnitud" es llave para el valor real que se sumará a la esperanza al realizar un cambio.
        HopeModifiers = new Dictionary<sbyte, float>();

        HopeModifiers.Add(-5, -30);
        HopeModifiers.Add(-4, -20);
        HopeModifiers.Add(-3, -10);
        HopeModifiers.Add(-2, -5);
        HopeModifiers.Add(-1, -1);

        HopeModifiers.Add(1, 1);
        HopeModifiers.Add(2, 5);
        HopeModifiers.Add(3, 10);
        HopeModifiers.Add(4, 20);
        HopeModifiers.Add(5, 30);

        PhaseInterval = MaxHope / (TotalHopePhases - 1);
        NeutralPhase = (byte)Mathf.Floor((TotalHopePhases - 1) / 2);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Initialized)
        {
            UpdateHopePhase();
            UpdateCombatFactor();
            CanvasHope.SetActive(GameManager.Instance.GameState == GAME_STATE.COMBAT);
            if (CanvasHope.activeSelf)
            {
                HopeBarFill.fillAmount = PartyHope / 100;
            }
            else
            {
                //Debug.Log("CanvasHope desactivado");
            }
        }
    }
    

    //Método que modifica la esperanza en base a la magnitud especificada
    public void ChangeHope(sbyte magnitude, string changeReason = "")
    {
        if(!HopeModifiers.ContainsKey(magnitude))
        {
            if(magnitude == 0)
            {
                Debug.Log("La magnitud no puede ser 0.");
            }
            else
            {
                Debug.Log("La magnitud especificada está fuera de rango.");
            }
            return;
        }

        float value = HopeModifiers[magnitude];

        PartyHope += value;

        if(PartyHope > MaxHope) { PartyHope = MaxHope; }
        if(PartyHope < MinHope) { PartyHope = MinHope; }

        Debug.Log(changeReason + " (" + value + ") | Esperanza del grupo: " + PartyHope);
    }

    //Este método permite cambiar la esperanza especificando el valor exacto a adicionar
    public void ChangeHopeDirectly(float value, string changeReason = "")
    {
        PartyHope += value;

        if (PartyHope > MaxHope) { PartyHope = MaxHope; }
        if (PartyHope < MinHope) { PartyHope = MinHope; }

        Debug.Log(changeReason + " (" + value + ") | Esperanza del grupo: " + PartyHope);
    }

    //Actualiza la fase actual de esperanza en base a la esperanza del grupo
    public void UpdateHopePhase()
    {
        CurrentHopePhase = (byte)Mathf.Floor(PartyHope / PhaseInterval);
        //Debug.Log("Esperanza: " + PartyHope + " | Fase: " + CurrentHopePhase);
    }

    //Actualiza el multiplicador de daño en base a fase de esperanza actual
    public void UpdateCombatFactor()
    {
        sbyte power = (sbyte)(CurrentHopePhase - NeutralPhase);
        CombatFactor = Mathf.Pow(BaseCombatFactor, power);
    }

    //Pensado para llamarse en combate y, así, evitar incongruencias
    public float GetHopeFactor()
    {
        UpdateHopePhase();
        UpdateCombatFactor();

        return CombatFactor;
    }

    public void InitializeHope()
    {
        PartyHope = 50;

        var hopeBar = Instantiate(HopeBar, CanvasHope.transform);

        RectTransform rectTransform = hopeBar.GetComponent<RectTransform>();
        //hopeBar.transform.SetParent(CanvasHope.transform);

        var images = GetComponentsInChildren<Image>();
        foreach(Image image in images)
        {
            if(image.type == Image.Type.Filled)
            {
                HopeBarFill = image;
                break;
            }
        }

        CanvasHope.SetActive(false);

        Initialized = true;
    }

    public void ResetHope()
    {
        PartyHope = 0;
        CurrentHopePhase = 0;
        Initialized = false;
    }

    //Puede que sea mejor aplicar los cambios de esperanza con éstos métodos
    public void StoreHopeChanges() { }
    public void ApplyHopeChanges() { }
    public void DiscardStoredHopeChanges() { }
}
