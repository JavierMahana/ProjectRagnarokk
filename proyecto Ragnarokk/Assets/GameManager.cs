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
    EXPLORATION
}

public class GameManager : Singleton<GameManager>
{
    //por ahora se almacenan todas las floors. Si es que se implementan mas pisos es necesario crear una lista de floors por piso.
    public List<Floor> AllFloors = new List<Floor>();
    public Floor CurrentFloor { get; set; }
    //[HideInInspector]
    //public bool FloorNeedToBeLoaded { get; set; }

    public GAME_STATE GameState
    {
        get
        {
            if (FindObjectOfType<CombatManager>())
            {
                return GAME_STATE.COMBAT;
            }
            else if (FindObjectOfType<ExplorationState>())
            {
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

    //public GameObject UIManager;

    //este es el combate actual
    [HideInInspector]
    public CombatEncounter currentEncounter;


    public List<Weapon> AllWeapons = new List<Weapon>();

    public List<FighterData> AllPlayerFighterDatas = new List<FighterData>();
    public List<CombatEncounter> AllEncounters = new List<CombatEncounter>();

    public List<Fighter> Enemies = new List<Fighter>();
    public List<FighterSelect> EnemyButtons = new List<FighterSelect>();

    //DontDestroyOnLoad
    //objetos de los fighters del player.
    [ReadOnly]
    public List<PlayerFighter> PlayerFighters = new List<PlayerFighter>();
    public List<FighterSelect> PlayerButtons = new List<FighterSelect>();

    public GameObject PlayerOnTurn;

    public bool ConfirmationClick;


    private void Update()
    {

        PlayerFighters.Clear();
        var pFighter = FindObjectsOfType<PlayerFighter>(false);
        foreach (var item in pFighter)
        {
            PlayerFighters.Add(item);
        }

       

    }

    public void StartFloor()
    {
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


    public void SetDataToFighterGO(GameObject fighterGO, FighterData data, bool playerFighter = false)
    {

        SpriteRenderer renderer;
        Fighter fighterComp;

        if (fighterGO.TryGetComponent(out renderer) && fighterGO.TryGetComponent(out fighterComp))
        {
            renderer.sprite = data.Sprite;

            fighterComp.Name = data.Name;
            fighterComp.Atack = data.Atack;
            fighterComp.Defense = data.Defense;
            fighterComp.Speed = data.Speed;
            fighterComp.MaxHP = data.MaxHP;
            fighterComp.CurrentHP = fighterComp.MaxHP;

            fighterComp.Type = data.Type;

            for (int i = 0; i < fighterComp.Weapons.Length; i++)
            {
                    //Debug.Log("Arma " + i);
                fighterComp.Weapons[i] = data.DefaultWeapons[i];
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

   
}
