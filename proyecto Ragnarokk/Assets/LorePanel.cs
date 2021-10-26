using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LorePanel : MonoBehaviour
{
    //private Sprite sprite = null;
    //private string text = "";

    public Text textComp;
    public Image imageComp;

    public void UpdateContent(Sprite sprite, string text)
    {
        if (textComp != null)
        {
            textComp.text = text;
        }
        else
        {
            Debug.LogError("El panel de lore no puede funcionar sin asignar el texto.");
        }

        if (imageComp != null)
        {
            imageComp.sprite = sprite;
        }
        else
        {
            Debug.LogError("El panel de lore no puede funcionar sin asignar la imagen.");
        }

    }
}
