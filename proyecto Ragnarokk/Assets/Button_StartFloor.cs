using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_StartFloor : MonoBehaviour
{
    public void StartFloor()
    {
        GameManager.Instance.StartFloor();
    }
}
