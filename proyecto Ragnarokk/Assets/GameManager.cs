using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;


//aca el jugador elije sus peleadores y ahi el juego le deja:
//empezar un encuentro
//curar a sus personajes
//elegir nuevos combatientes

public enum GAME_STATE
{
    PREGAME,
    COMBAT,
    EXPLORATION,
    MENU,
    SHOP,
    TREASURE,
    CREDITS
}

public class GameManager : Singleton<GameManager>
{
    //por ahora se almacenan todas las floors. Si es que se implementan mas pisos es necesario crear una lista de floors por piso.
    public List<Floor> AllFloors = new List<Floor>();
    public Floor CurrentFloor { get; set; }
    //[HideInInspector]
    //public bool FloorNeedToBeLoaded { get; set; }

    public int CurrentMoney;
    public bool OnBossFight = false;

    public GAME_STATE GameState
    {
        get
        {
            if (FindObjectOfType<VictoryManager>())
            {
                return GAME_STATE.CREDITS;
            }

            if (FindObjectOfType<ShopManager>())
            {
                return GAME_STATE.SHOP;
            }

            else if (FindObjectOfType<TreasureState>())
            {
                return GAME_STATE.TREASURE;
            }

            else if (FindObjectOfType<ShopState>())
            {
                return GAME_STATE.SHOP;
            }

            else if (FindObjectOfType<CombatManager>())
            {
                return GAME_STATE.COMBAT;
            }

            else if (FindObjectOfType<ExplorationState>())
            {
                if (FindObjectOfType<GeneralMenu>(true).MenuDropdown.value > 0)
                {
                    return GAME_STATE.MENU;
                }
                return GAME_STATE.EXPLORATION;
            }
           
            else
            {
                return GAME_STATE.PREGAME;
            }
        }
    }

    public bool InFightingScene 
    { 
        get 
        {
            if (FindObjectOfType<CombatManager>())
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
    }

    [HideInInspector]
    public List<Item> currentTreasureItems = new List<Item>();
    [HideInInspector]
    public int treasureRoomMoney;

    //este es el combate actual
    [HideInInspector]
    public CombatEncounter currentEncounter;

    public Dictionary<Room, ShopData> InitializedShopsInFloor = new Dictionary<Room, ShopData>();
    public ShopData CurrShopData;

    public List<Weapon> AllWeapons = new List<Weapon>();

    public List<FighterData> AllPlayerFighterDatas = new List<FighterData>();
    public List<CombatEncounter> AllEncounters = new List<CombatEncounter>();

    public List<Fighter> Enemies = new List<Fighter>();
    public List<FighterSelect> EnemyButtons = new List<FighterSelect>();
    
    public List<Consumible> AllConsumibles = new List<Consumible>();


    //public List<CombatState> AllCombatStates = new List<CombatState>();

    //DontDestroyOnLoad
    //objetos de los fighters del player.
    [ReadOnly]
    public List<PlayerFighter> PlayerFighters = new List<PlayerFighter>();
    public List<FighterSelect> PlayerButtons = new List<FighterSelect>();

    public GameObject PlayerOnTurn;


    // estos bool son para el tipo de acción que el jugador escogió en su turno
    [HideInInspector]
    public bool ConfirmationClick;
    [HideInInspector]
    public bool OnAttack;
    [HideInInspector]
    public bool OnConsumible;
    [HideInInspector]
    public bool OnDefense;
    [HideInInspector]
    public bool OnFleeCombat;


    // en el start se setea la configuracion del jugador.
    public void Start()
    {
        // comentar la linea de abajo para que que se guarden los cambios que el jugador realice
        // El valor de un PlayerPref cuando no se encuentra, es 0, es el valor incial default
        PlayerPrefs.SetInt("firstTime", 0);
        // si es la primera vez que se inicia el juego, la data cargada es ésta, de lo contrario
        // se carga la data de Options.
        if (PlayerPrefs.GetInt("firstTime") == 0)
        {
            PlayerPrefs.SetFloat("audioGeneral", 90f);
            PlayerPrefs.SetFloat("audioSFX", 80f);
            PlayerPrefs.SetFloat("audioMusic", 70f);
            PlayerPrefs.SetFloat("audioAmbient", 60f);

            PlayerPrefs.SetFloat("combatDescriptorSpeed", 5f);

            PlayerPrefs.SetInt("Fullscreen", 0);
            PlayerPrefs.SetInt("width", Screen.currentResolution.width);
            PlayerPrefs.SetInt("height", Screen.currentResolution.height);

            //PlayerPrefs.SetInt("firstTime", 1);
            PlayerPrefs.SetInt("currentFloor", 0);
        }
        
        // se aplica la configuración del jugador
        SetPlayerConfiguration();
        FindObjectOfType<AudioManager>().CheckMusic();

    }

    public void SetPlayerConfiguration()
    {
        var fullscreen = PlayerPrefs.GetInt("Fullscreen") == 0 ? true : false;
        var width = PlayerPrefs.GetInt("width");
        var height = PlayerPrefs.GetInt("height");

        Screen.SetResolution(width, height, fullscreen);

        FindObjectOfType<AudioManager>().UpdateAudioParameters();


    }

    private void Update()
    {
        //Debug.Log($"{GameState}");

        PlayerFighters.Clear();
        var pFighter = FindObjectsOfType<PlayerFighter>(true);
        foreach (var item in pFighter)
        {
            PlayerFighters.Add(item);
        }

        if (!PartyIsComplete())
        {
            HopeManager.Instance.ResetHope();
        }
        else if (!HopeManager.Instance.Initialized)
        {
            HopeManager.Instance.InitializeHope();
        }


    }

    public void StartFloor()
    {
        int floorNumber = PlayerPrefs.GetInt("currentFloor");
        floorNumber++;
        PlayerPrefs.SetInt("currentFloor", floorNumber);

        InitializedShopsInFloor.Clear();

        int count = AllFloors.Count;
        if (count > 0)
        {
            int selected = Random.Range(0, count);
            CurrentFloor = AllFloors[selected];
            //FloorNeedToBeLoaded = true;
            //Debug.Log(FloorNeedToBeLoaded);

            SceneChanger.Instance.ChangeScene("Exploration");
        }
        else
        {
            Debug.LogError("You need to put floors in the game manager!");
            return;
        }
    }

    public void DeletePlayerFighters()
    {
        var pfs = FindObjectsOfType<PlayerFighter>();
        foreach (var pf in pfs)
        {
            Destroy(pf.gameObject);
        }
    }

    //esto debe revivr los fighter si es q estan muertos.
    public void HealPlayerFighters()
    {
        var pfs = FindObjectsOfType<PlayerFighter>();
        foreach (var pf in pfs)
        {
            var fighter = pf.GetComponent<Fighter>();
            fighter.CurrentHP = fighter.MaxHP;
            fighter.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void ShowPlayerFighters(bool show)
    {
        var fightersaliados = PlayerFighters;
        foreach (var fighter in fightersaliados)
        {
            fighter.gameObject.SetActive(show);
        }
    }

    public void SetDataToFighterGO(GameObject fighterGO, FighterData data, bool playerFighter = false)
    {

        SpriteRenderer renderer;
        Fighter fighterComp;

        if (fighterGO.TryGetComponent(out renderer) && fighterGO.TryGetComponent(out fighterComp))
        {
            renderer.sprite = data.Sprite;

            fighterComp.Init(data);

            for (int i = 0; i < fighterComp.Weapons.Length; i++)
            {
                    //Debug.Log("Arma " + i);
                fighterComp.Weapons[i] = data.DefaultWeapons[i];
                fighterComp.WeaponCooldowns[i] = 0;
            }
        }
        else
        {
            Debug.LogError("the fighter prefab needs: SpriteRenderer, Fighter,  Components");
            return;
        }

        //si no tienen el componente PlayerFighter se le agrega.
        if (playerFighter)
        {
            PlayerFighter pFighter;
            if (!fighterGO.TryGetComponent(out pFighter))
            {
                fighterGO.AddComponent<PlayerFighter>();
            }

            //fighterGO.GetComponent<PlayerFighter>();
            PlayerFighters.Add(fighterGO.GetComponent<PlayerFighter>());


        }
    }

    public bool PartyIsComplete()
    {
        return PlayerFighters.Count == 3;
    }

    public void SaveNQuit() 
    {
        //save data and exit game
        Application.Quit();
    }

    public bool CheckTutorialComplete()
    {
        // si es primera vez que se abre el juego
        if (PlayerPrefs.GetInt("tutorialComplete") == 0) { return true; }
        return false;
    }

    public void TutorialComplete()
    {
        if (CheckTutorialComplete())
        {
            PlayerPrefs.SetInt("tutorialComplete", 1);
        }
    }

    public void CheckOnLoadScene()
    {
        FindObjectOfType<AudioManager>().CheckMusic();
    }

    public void RestartGame()
    {
        CurrentMoney = 0;
        PlayerFighters.Clear();
    }
}
