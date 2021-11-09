using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConsumibleType
{
    HEALTH_REGEN_1,
    REVIVE_1
}

[CreateAssetMenu(fileName = "New Consumible", menuName = "Consumibles")]
public class Consumible : Item
{
    public ConsumibleType type;

    // Aqui se añade especifica la funcion del consumible, al terminar de usarse debería destruirse
    // su destruiccion y eliminación de la lista de consumibles debería estar en el gameManager.
    public void OnUse(Fighter user)
    {        

        switch (type)
        {
            #region objeto 1
            case ConsumibleType.HEALTH_REGEN_1: if (user.CurrentHP != user.MaxHP)
                    {
                        user.CurrentHP += 15;
                        if (user.CurrentHP > user.MaxHP)
                        {
                            user.CurrentHP = user.MaxHP;
                        }
                        ItemUsedCorrectly();
                        
                    }
                    else
                    {
                        ItemNotUsedCorrectly();
                    }
                break;
            #endregion

            #region objeto 2
            case ConsumibleType.REVIVE_1:
                
                if (user.CurrentHP <= 0)
                {
                    user.CurrentHP = user.MaxHP;
                    ItemUsedCorrectly();
                }
                else
                {
                    ItemNotUsedCorrectly();
                }
                break;
            #endregion
            default: Debug.Log("El item no tiene una función asignada");
                break;
        }

    }

    private void ItemUsedCorrectly()
    {
        // se elimina el consumible recien utilizado de la lista de consumibles
        GameManager.Instance.AllConsumibles.Remove(this);

        // destruye la instancia porque el objeto fue usado
        Destroy(this);

        var combatManager = FindObjectOfType<CombatManager>();
        var consumiblePanel = FindObjectOfType<ConsumiblePanel>();

        // se termina el turno
        if (GameManager.Instance.GameState == GAME_STATE.COMBAT)
        {
            combatManager.ActionDone = true;
        }

        // elimina el objeto del panel en el menu
        else if (GameManager.Instance.GameState == GAME_STATE.MENU)
        {
            consumiblePanel.SelectedConsumible = null;
            consumiblePanel.SetUpPanel();
        }

        

    }

    private void ItemNotUsedCorrectly()
    {
        Debug.Log("El objeto no puede utilizarse");

        // si no se cumplio la condicion para utilizar el consumible, el consumible se deselecciona
        var combatManager = FindObjectOfType<CombatManager>();
        if(combatManager != null)
        {
            combatManager.SelectedConsumible = null;
        }
        
    }
}
