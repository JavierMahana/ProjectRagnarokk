using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCanvas : MonoBehaviour
{
    public TeamFighterPanel TeamFighterPanel1;
    public TeamFighterPanel TeamFighterPanel2;
    public TeamFighterPanel TeamFighterPanel3;

    // Start is called before the first frame update
    void Start()
    {
        Fighter f1 = GameManager.Instance.PlayerFighters[0].GetComponent<Fighter>();
        Fighter f2 = GameManager.Instance.PlayerFighters[1].GetComponent<Fighter>();
        Fighter f3 = GameManager.Instance.PlayerFighters[2].GetComponent<Fighter>();
        TeamFighterPanel1.fillPanel(f1);
        TeamFighterPanel2.fillPanel(f2);
        TeamFighterPanel3.fillPanel(f3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
