using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Treasure Room")]
public class TreasureRoom : RoomData
{
    public List<Item> AllPosibleTreasures = new List<Item>();
    public int MoneyAmmount = 50;
    //public int moneyAmount
    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {
        gameManager.currentTreasureItems.Clear();

        List<int> numbersAlreadyUsed = new List<int>();
        int count = AllPosibleTreasures.Count;
        if (count == 0)
            Debug.LogError("The tresure room must have posibles treasures."); 
        
        for (int i = 0; i < 3; i++)
        {
            int selection;
            if (count > 3)//esto es para esegurarse que no salgan items repetidos como recompenza
            {
                do //se loopea hasta que se encuentre un item que no se ha usado.
                {
                    selection = Random.Range(0, count - 1);
                } while (numbersAlreadyUsed.Contains(selection));
            }
            else 
            {
                selection = Random.Range(0, count - 1);
            }
            


            gameManager.currentTreasureItems.Add(AllPosibleTreasures[selection]);
            numbersAlreadyUsed.Add(selection);
        }// se termina la seleccion de tesoros.
        gameManager.treasureRoomMoney = MoneyAmmount;

        room.MarkAsCurrent();
        room.MarkAsCleared();

        sceneChanger.ChangeScene("TreasureRoom");
    }
}
