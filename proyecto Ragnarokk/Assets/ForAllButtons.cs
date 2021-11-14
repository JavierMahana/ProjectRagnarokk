using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForAllButtons : MonoBehaviour, IPointerClickHandler
{
    private bool IsButtonPressed;
    private Color Pressed;
    private Color Normal;

    public void Start()
    {
        float r= 0;
        float g= 0;
        float b= 0;

        IsButtonPressed = false;

        var button = GetComponent<Button>();

        if (button != null) 
        {
            r = 1f;
            g = 120/255f;
            b = 0;
            Normal = new Color(r, g, b, 0.8f);

            r = 0;
            g = 200/255f;
            b = 130/255f;
            Pressed = new Color(r, g, b, 1);

            button.image.color = Normal;
        }       
    }

    public void ButtonSound()
    {
        FindObjectOfType<AudioManager>().PlayButtonSound();
    }


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        
        if(gameObject.TryGetComponent(out Button b))
        {
            if(b.interactable == true)
            {
                var otherButtons = FindObjectsOfType<Button>();
                foreach (Button ob in otherButtons)
                {
                    if (ob.gameObject.activeSelf && ob.TryGetComponent(out ForAllButtons fa) && ob != b)
                        fa.UnPressButton(ob);
                }

                if (!IsButtonPressed)
                {
                    PressButton(b);
                    //suar Buttonsound actual aqui
                }
                else
                {
                    UnPressButton(b);
                    //añadir sonido al desactivar
                }

                ButtonSound();
            }
            else
            {
                b.image.color = Normal;
            }
            
        }
        else if(gameObject.TryGetComponent(out Dropdown d))
        {
            ButtonSound();
        }
        else if (gameObject.TryGetComponent(out Slider s))
        {
            //añadir efecto de slider
        }
        else
        {
            Debug.Log("ForAllButtons no encuentra un componente necesario");
        }

        Debug.Log(gameObject.name);

       
    }

    public void PressButton(Button b)
    {  
        b.image.color = Pressed;
        IsButtonPressed = true;
    }

    public void UnPressButton(Button b)
    {
        b.image.color = Normal;
        IsButtonPressed = false;
    }
}
