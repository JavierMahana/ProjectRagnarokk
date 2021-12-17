using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConsumibleType
{
    HEALTH_REGEN_1,
    REVIVE_1,
    REVIERE_1
}

[CreateAssetMenu(fileName = "New Consumible", menuName = "Consumibles")]
public class Consumible : Item
{
    public ConsumibleType type;
    [Range(1, 3)]
    public byte Power;

    // Aqui se a�ade especifica la funcion del consumible, al terminar de usarse deber�a destruirse
    // su destruiccion y eliminaci�n de la lista de consumibles deber�a estar en el gameManager.
    public void OnUse(Fighter user, FighterSelect button = null)
    {
        if (button != null) { user = button.Fighter; }
        
        var combatDescriptor = FindObjectOfType<CombatDescriptor>();
        var combatManager = FindObjectOfType<CombatManager>();
 
        string userName = "";
        string fighterInTurnName = "";
        Fighter fighterInTurn = null;
        if (combatManager != null)
        {
            if (combatManager.PlayerFighters.Contains(user))
            {
                userName = user.RealName;
            }
            else
            {
                userName = user.Name;
            }


            fighterInTurn = combatManager.ActiveFighter;
            if (combatManager.PlayerFighters.Contains(fighterInTurn))
            {
                fighterInTurnName = fighterInTurn.RealName;
            }
            else
            {
                fighterInTurnName = fighterInTurn.Name;
            }
            
        }

        switch (type)
        {
            #region objeto 1
            case ConsumibleType.HEALTH_REGEN_1:
                if (user.CurrentHP != user.MaxHP && user.CurrentHP > 0)
                {
                    int recoveryValue, realRecoveryValue;
                    switch(Power)
                    {
                        case 1: recoveryValue = 30; break;
                        case 2: recoveryValue = 70; break;
                        case 3: recoveryValue = 150; break;

                        default: recoveryValue = 30; break;
                    }

                    realRecoveryValue = recoveryValue;
                    
                    if (user.CurrentHP + recoveryValue > user.MaxHP)
                    {
                        realRecoveryValue = user.MaxHP - user.CurrentHP;
                    }
                    user.CurrentHP += realRecoveryValue;

                    if (combatManager != null)
                    {
                        button.ShowText(false, realRecoveryValue, false, 0); 
                        
                        combatDescriptor.Clear();

                        string useLine = "";
                        if (user.Equals(fighterInTurn)) { useLine = fighterInTurnName + " uses " + Name; }
                        else { useLine = fighterInTurnName + " uses " + Name + " on " + userName; }
                        combatDescriptor.AddTextLine(useLine);
                        combatDescriptor.AddTextLine(userName + " recovers " + realRecoveryValue + " HP");
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
                    int recoveryValue;
                    switch (Power)
                    {
                        case 1: recoveryValue = Mathf.CeilToInt(user.MaxHP / 3f); break;
                        case 2: recoveryValue = Mathf.CeilToInt(user.MaxHP / 2f); break;
                        case 3: recoveryValue = user.MaxHP; break;

                        default: recoveryValue = Mathf.CeilToInt(user.MaxHP / 3f); break;
                    }

                    user.CurrentHP = recoveryValue;

                    if (combatManager != null)
                    {
                        button.ShowText(false, recoveryValue, false, 0);

                        combatManager.LiftPlayerFighter(user);

                        combatDescriptor.Clear();
                        combatDescriptor.AddTextLine(fighterInTurnName + " uses " + Name + " on " + userName);
                        combatDescriptor.AddTextLine(userName + " has risen!");
                    }

                    ItemUsedCorrectly();
                }
                else
                {
                    ItemNotUsedCorrectly();
                }
                break;
            #endregion

            #region objeto 3
            case ConsumibleType.REVIERE_1:
                var hope = HopeManager.Instance;
                if ( hope.PartyHope < hope.Limit)
                {
                    int recoveryValue, realRecoveryValue;
                    switch (Power)
                    {
                        case 1: recoveryValue = 5; break;
                        case 2: recoveryValue = 10; break;
                        case 3: recoveryValue = 25; break;

                        default: recoveryValue = 5; break;
                    }

                    realRecoveryValue = recoveryValue;
                    if(hope.PartyHope + recoveryValue > hope.Limit)
                    {
                        realRecoveryValue = (int)(hope.Limit - hope.PartyHope);
                    }

                    hope.PartyHope += realRecoveryValue;

                    if (combatManager != null)
                    {
                        combatDescriptor.Clear();

                        string useLine = "";
                        if (user.Equals(fighterInTurn)) { useLine = fighterInTurnName + " uses " + Name; }
                        else { useLine = fighterInTurnName + " uses " + Name + " on " + userName; }
                        combatDescriptor.AddTextLine(useLine);
                        combatDescriptor.AddTextLine("The party revers "+ realRecoveryValue + " hope!");

                    }
                    ItemUsedCorrectly();
                }
                else
                {
                    ItemNotUsedCorrectly();
                }

                break;
            #endregion
            default: Debug.Log("El item no tiene una funci�n asignada");
                break;
        }

    }

    private void ItemUsedCorrectly()
    {
        var combatManager = FindObjectOfType<CombatManager>();
        var consumiblePanel = FindObjectOfType<ConsumiblePanel>();

        // se elimina el consumible recien utilizado de la lista de consumibles
        GameManager.Instance.AllConsumibles.Remove(this);

        // destruye la instancia porque el objeto fue usado

        // se termina el turno
        if (GameManager.Instance.GameState == GAME_STATE.COMBAT)
        {
            combatManager.ShowActionCanvas(false);
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
            combatManager.SetlDescriptorText("You can't use that item!");
            combatManager.SelectedConsumible = null;
        }
        
    }
}
