using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationState : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var gm = GameManager.Instance;

        gm.CheckOnLoadScene();
        gm.ShowPlayerFighters(false);


        if (gm.InFloorEnd)
        {
            gm.InFloorEnd = false;            
            gm.StartFloor(gm.FloorToLoadInFloorEnd);
        }
    }

}
