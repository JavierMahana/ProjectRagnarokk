using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreManager : MonoBehaviour
{
    [Multiline]
    public List<string> Lore = new List<string>();
    List<bool> Showed = new List<bool>();

    public void Init()
    {
        foreach(string lore in Lore)
        {
            Showed.Add(false);
        }

        Debug.Log($"Count Lore: {Lore.Count}");
        Debug.Log($"Count Showed: {Showed.Count}");
    }

    public string FirstLine(string text)
    {
        return text.Split('\n')[0];
    }


    public string GetNewLore()
    {
        /*
        Debug.Log("Antes:");
        for(int i = 0; i<Lore.Count || i<Showed.Count; i++)
        {
            string lore = i < Lore.Count ? Lore[i] : "No lore";
            string showed = i < Showed.Count ? Showed[i].ToString() : "No boolean assigned";
            Debug.Log(FirstLine(lore) + " -> " + showed);
        }
        */

        List<int> indexes = new List<int>();
        if(!Showed.Contains(false))
        {
            for(int i = 0; i<Showed.Count; i++)
            {
                Showed[i] = false;
            }
        }

        for(int i = 0; i<Lore.Count; i++)
        {
            if(!Showed[i])
            {
                indexes.Add(i);
            }
        }

        int index = indexes[Random.Range(0, indexes.Count)];
        Showed[index] = true;

        /*
        Debug.Log("Despues:");
        for (int i = 0; i < Lore.Count || i < Showed.Count; i++)
        {
            string lore = i < Lore.Count ? Lore[i] : "No lore";
            string showed = i < Showed.Count ? Showed[i].ToString() : "No boolean assigned";
            Debug.Log(FirstLine(lore) + " -> " + showed);
        }
        */

        return Lore[index];
    }
}
