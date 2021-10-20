using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterManager : MonoBehaviour
{
    public GameObject RestartButton;
    public GameObject StartButton;
    public GameObject SelectFighterButtons;



    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.PlayerFighters.Count >= 3)
        {
            SelectFighterButtons.SetActive(false);
            StartButton.SetActive(true);
        }
        else
        {
            SelectFighterButtons.SetActive(true);
            StartButton.SetActive(false);
        }

        if (GameManager.Instance.PlayerFighters.Count >= 1)
        {
            RestartButton.SetActive(true);
        }
        else
        {
            RestartButton.SetActive(false);
        }
    }

    
}
