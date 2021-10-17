using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Floor : SerializedMonoBehaviour
{
    [BoxGroup("Sos")]
    public int[,] areglo = new int[10,10];
}
