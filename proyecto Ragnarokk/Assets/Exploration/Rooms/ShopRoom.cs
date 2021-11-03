using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Exploration/Room/Shop Room")]
public class ShopRoom : RoomData
{
    public List<Weapon> AllPosibleWeapons = new List<Weapon>();

    public List<Consumible> AllPosibleConsumibles = new List<Consumible>();


    public override void LoadRoom(GameManager gameManager, SceneChanger sceneChanger, Room room)
    {

        ShopData shopData;
        if (GameManager.Instance.InitializedShopsInFloor.TryGetValue(room, out shopData))
        {
            //load with the data shop
            GameManager.Instance.CurrShopData = shopData;
        }
        else
        {

            var selectedWeapons = new Weapon[3];
            var selectedConsumibles = new Consumible[3];

            List<int> numbersAlreadyUsed = new List<int>();
            //weapon select.
            for (int i = 0; i < 3; i++)
            {
                
                int selection;
                if (AllPosibleWeapons.Count > 3)//esto es para esegurarse que no salgan items repetidos como recompenza
                {
                    do //se loopea hasta que se encuentre un item que no se ha usado.
                    {
                        selection = Random.Range(0, AllPosibleWeapons.Count - 1);
                    } while (numbersAlreadyUsed.Contains(selection));
                }
                else
                {
                    selection = Random.Range(0, AllPosibleWeapons.Count - 1);
                }

                numbersAlreadyUsed.Add(selection);

                selectedWeapons[i] = AllPosibleWeapons[selection];
            }

            //consumible select.
            for (int i = 0; i < 3; i++)
            {
                int selection;
                selection = Random.Range(0, AllPosibleConsumibles.Count - 1);

                selectedConsumibles[i] = AllPosibleConsumibles[selection];
            }


            shopData = new ShopData(selectedWeapons[0], selectedWeapons[1], selectedWeapons[2],
                                    selectedConsumibles[0], selectedConsumibles[1], selectedConsumibles[2]);

            GameManager.Instance.InitializedShopsInFloor.Add(room, shopData);
            GameManager.Instance.CurrShopData = shopData;
        }


        //room.MarkAsCurrent();
        //room.MarkAsCleared();

        sceneChanger.ChangeScene("ShopRoom");
    }
}
