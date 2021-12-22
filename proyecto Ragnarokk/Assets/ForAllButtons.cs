using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ForAllButtons : MonoBehaviour, IPointerClickHandler
{
    private bool IsButtonPressed;
    public Color Pressed;
    public Color Normal;

    public bool staysPressed = true;
    public bool overrideColors = false;
    public void Start()
    {
        float r= 0;
        float g= 0;
        float b= 0;

        IsButtonPressed = false;

        var button = GetComponent<Button>();

        if (button != null && !overrideColors) 
        {
            /*
            r = 0f;
            g = 24/255f;
            b = 94/255f;
            Normal = new Color(r, g, b, 0.8f);
            */
            Normal = Color.white;

            r = 120/255f;
            g = 190/255f;
            b = 1f;
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

                if (!IsButtonPressed && staysPressed)
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
            //Debug.Log("ForAllButtons no encuentra un componente necesario");
        }

        //Debug.Log(gameObject.name);

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
