using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Derrota : MonoBehaviour
{
    void Start()
    {
        var pfs = GameManager.Instance.PlayerFighters;
        foreach(PlayerFighter pf in pfs)
        {
            pf.GetComponentInChildren<SpriteRenderer>().enabled = false;
            pf.GetComponentInChildren<Image>().enabled = false;
        }
        
    }
}
