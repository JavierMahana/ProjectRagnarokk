using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveNQuit : MonoBehaviour
{
    public string Panelname = "Save & Quit";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        GameManager.Instance.SaveNQuit();
    }
}
