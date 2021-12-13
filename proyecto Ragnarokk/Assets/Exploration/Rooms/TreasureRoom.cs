using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Treasure Room")]
public class TreasureRoom : RoomData
{
    public List<Item> CommonTreasures = new List<Item>();
    public List<Item> RareTreasures = new List<Item>();
    public List<Item> VeryRareTreasures = new List<Item>();
    public List<Item> LegendaryTreasures = new List<Item>();

    private int RareGoal = 50;
    private int VeryRareGoal = 80;
    private int LegendaryGoal = 95;

    public List<Item> AllPosibleTreasures = new List<Item>();
    public int MoneyAmmount = 50;
    //public int moneyAmount
    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        gameManager.currentTreasureItems.Clear();

        //List<int> numbersAlreadyUsed = new List<int>();
        int count = AllPosibleTreasures.Count;
        if (count == 0)
            Debug.LogError("The tresure room must have posibles treasures.");

        List<List<Item>> RemainingT = new List<List<Item>>();

        List<Item> RemainingCommonT     = new List<Item>(CommonTreasures);
        RemainingT.Add(RemainingCommonT);
        List<Item> RemainingRareT       = new List<Item>(RareTreasures);
        RemainingT.Add(RemainingRareT);
        List<Item> RemainingVeryRareT   = new List<Item>(VeryRareTreasures);
        RemainingT.Add(RemainingVeryRareT);
        List<Item> RemainingLegendaryT  = new List<Item>(LegendaryTreasures);
        RemainingT.Add(RemainingLegendaryT);

        for (int i = 0; i < 3; i++)
        {
            int TListIndex;
            do
            {
                int rarityValue = Random.Range(0, 100);

                if (rarityValue >= LegendaryGoal)       { TListIndex = 3; }
                else if (rarityValue >= VeryRareGoal)   { TListIndex = 2; }
                else if (rarityValue >= RareGoal)       { TListIndex = 1; }
                else                                    { TListIndex = 0; }

            } while (RemainingT[TListIndex].Count == 0);
            

            int selection;
            selection = Random.Range(0, RemainingT[TListIndex].Count);

            gameManager.currentTreasureItems.Add(RemainingT[TListIndex][selection]);
            //numbersAlreadyUsed.Add(selection);

            if (count > 3)//esto es para esegurarse que no salgan items repetidos como recompenza
            {
                Debug.Log($"Restantes lista {TListIndex}:");
                foreach (Item item in RemainingT[TListIndex])
                {
                    Debug.Log(item.Name);
                }
                //Debug.Log($"Lista {TListIndex} Antes: {RemainingT[TListIndex].Count}");
                RemainingT[TListIndex].RemoveAt(selection);
                //Debug.Log($"Lista {TListIndex} Despues: {RemainingT[TListIndex].Count}");
                /*
                do //se loopea hasta que se encuentre un item que no se ha usado.
                {
                    selection = Random.Range(0, count - 1);
                } while (numbersAlreadyUsed.Contains(selection));
                */
            }
            else 
            {
                //selection = Random.Range(0, count - 1);
            }

            
        }// se termina la seleccion de tesoros.
        gameManager.treasureRoomMoney = MoneyAmmount;

        room.MarkAsCurrent();
        room.MarkAsCleared();

        sceneChanger.ChangeScene("TreasureRoom");
    }
}
