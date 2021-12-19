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

    // Aqui se añade especifica la funcion del consumible, al terminar de usarse debería destruirse
    // su destruiccion y eliminación de la lista de consumibles debería estar en el gameManager.
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
                    const int recoveryValue = 30;
                    user.CurrentHP += recoveryValue;
                    if (user.CurrentHP > user.MaxHP)
                    {
                        user.CurrentHP = user.MaxHP;
                    }

                    if(combatManager != null)
                    {
                        button.ShowText(false, false, recoveryValue, false, 0); 
                        
                        combatDescriptor.Clear();

                        string useLine = "";
                        if (user.Equals(fighterInTurn)) { useLine = fighterInTurnName + " usa " + Name; }
                        else { useLine = fighterInTurnName + " usa " + Name + " en " + userName; }
                        combatDescriptor.AddTextLine(useLine);
                        combatDescriptor.AddTextLine(userName + " recupera " + recoveryValue + " Salud");

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

                    if (combatManager != null)
                    {
                        button.ShowText(false, false, user.MaxHP, false, 0);

                        combatManager.LiftPlayerFighter(user);

                        combatDescriptor.Clear();
                        combatDescriptor.AddTextLine(fighterInTurnName + " usa " + Name + " en " + userName);
                        combatDescriptor.AddTextLine( "!" + userName + " se ha levantado para luchar!");
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
                    hope.PartyHope += 25f;
                    if(hope.PartyHope > 100) { hope.PartyHope = hope.Limit; }

                    if (combatManager != null)
                    {

                        combatDescriptor.Clear();

                        int recovery = 25;

                        button.ShowText(false, true, recovery, false, 0);
                        string useLine = "";
                        if (user.Equals(fighterInTurn)) { useLine = fighterInTurnName + " usa " + Name; }
                        else { useLine = fighterInTurnName + " usa " + Name + " en " + userName; }
                        combatDescriptor.AddTextLine(useLine);
                        combatDescriptor.AddTextLine("¡El equipo recupera "+ recovery + "Esperanza!");

                    }
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
            combatManager.SetlDescriptorText("¡No puedes utilizar ese objeto!");
            combatManager.SelectedConsumible = null;
        }
        
    }
}
