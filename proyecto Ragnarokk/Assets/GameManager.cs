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
public class GameManager : Singleton<GameManager>
{
    
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

    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> EnemyButtons = new List<GameObject>();

    public List<GameObject> PlayerButtons = new List<GameObject>();


    //DontDestroyOnLoad
    //objetos de los fighters del player.
    [ReadOnly]
    public List<PlayerFighter> PlayerFighters = new List<PlayerFighter>();
    public GameObject PlayerOnTurn;


   
    private void Update()
    {

        PlayerFighters.Clear();
        var pFighter = FindObjectsOfType<PlayerFighter>(false);
        foreach (var item in pFighter)
        {
            PlayerFighters.Add(item);
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
            fighterComp.CurrentHP = data.MaxHP;

            for (int i = 0; i < fighterComp.Weapons.Length; i++)
            {
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

   

    //public void CreateFighter(int i)
    //{
    //    //if()
    //}
}
