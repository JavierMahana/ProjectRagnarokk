using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelect : MonoBehaviour
{
    public Button selfButton;

    public GameObject PlayerAssigned;
    void Start()
    {
        selfButton.gameObject.SetActive(false);
    }

    void Update()
    {
        //cambiar la primera condicion por revisar si es que es su turno.
        if(PlayerAssigned.GetComponent<Fighter>().Speed > 0 && !selfButton.gameObject.activeSelf)
        {
            selfButton.gameObject.SetActive(true);
        }

        if(PlayerAssigned.GetComponent<Fighter>().Speed <= 0 && selfButton.gameObject.activeSelf)
        {
            selfButton.gameObject.SetActive(false);
        }
    }
}
