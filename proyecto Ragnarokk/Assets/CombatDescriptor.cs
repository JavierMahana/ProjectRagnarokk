using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Clase encargada de manejar la caja de descripci�n de combate, describiendo gran parte de los sucesos de un combate, seg�n se le indica desde otras
//clases.
public class CombatDescriptor : MonoBehaviour
{
    //Clase de l�neas de texto que el descriptor deber� mostrar
    class TextLine
    {
        public string Line { private set; get; } //Texto
        public float PermanenceTime; //Tiempo que la l�nea permanecer� desde que es la l�nea principal
        public bool IsFirstLine; //Indica si ha sido declarada como l�nea principal

        public TextLine(string line, float permTime)
        {
            Line = line;
            PermanenceTime = permTime;
            IsFirstLine = false;
        }
    }

    public GameObject CanvasCombatDescriptor; //Prefab del canvas del descriptor (EN DESUSO)
    public TextMeshProUGUI Text; //Texto �nico del canvas, el cual muestra las l�neas de texto
    List<TextLine> TextLines; //Todas las l�neas que ser�n mostradas
    public bool TextIsEmpty { private set; get; }

    private void Awake()
    {
        //Text = CanvasCombatDescriptor.GetComponentInChildren<Text>();
        //Debug.Log(Text.text);
        TextLines = new List<TextLine>();
        TextIsEmpty = true;
    }

    private void Update()
    {
        TextIsEmpty = TextLines.Count == 0;
        Text.text = "";
        if (!TextIsEmpty)
        {
            //Actualizaci�n continua del texto mostrado, seg�n las l�neas de texto almacenadas
            foreach (TextLine textLine in TextLines)
            {
                string n = textLine.Equals(TextLines[0]) ? "" : "\n";
                string line = textLine.Line;
                Text.text = line + n + Text.text; //Las l�neas son mostradas desde abajo hacia arriba
            }

            //Cuando la primera l�nea de texto no ha sido declarada como tal, se hace, e inicia el tiempo de permanencia antes de desaparecer
            //Las l�neas permanentes NO se eliminan de esta forma.
            if (TextLines[0].PermanenceTime != 0  &&  !TextLines[0].IsFirstLine)
            {
                TextLines[0].IsFirstLine = true;
                StartCoroutine(RemoveLineDelayed(TextLines[0]));
            }
        }
    }

    //A�ade una l�nea de texto a la lista. No se a�adir�n strings vac�os, puesto que se generar�a un salto de l�nea innecesario. Se puede
    //especificar un tiempo de permanencia, pero, por defecto, �ste es de 1 segundo.
    //L�neas de texto cuyo PermanenceTime es 0 son l�neas permanentes, y se requiere limpiar el descriptor para quitarlas. Esto puede
    //generar acumulaci�n de l�neas tras una l�nea permanente si no se gestiona correctamente. Siempre que hayan l�neas permanentes
    //mostr�ndose, se debe limpiar el descriptor con su correspondiente m�todo antes de agregar nuevas l�neas temporales.
    public void AddTextLine(string line, float permanenceTime = 1)
    {
        if(line.Equals("")) { return; }
        TextLines.Add(new TextLine(line, permanenceTime));
    }

    //M�todo especial para mostrar de qui�n es el turno actual. Limpia el descriptor antes de a�adir la l�nea, la cual es PERMANENTE.
    public void ShowFighterInTurn(Fighter activeFighter, bool isPlayerFighter)
    {
        //Clear();

        string fName = isPlayerFighter? activeFighter.RealName : activeFighter.Name;
        string line = fName + "'s turn";
        TextLines.Add(new TextLine(line, 0));
    }

    //Elimina l�neas temporales, esperando el tiempo de permanencia correspondiente desde el momento en que se le declara como primera l�nea.
    //NO ELIMINA L�NEAS PERMANENTES
    IEnumerator RemoveLineDelayed(TextLine textLine)
    {
        yield return new WaitForSeconds(textLine.PermanenceTime / PlayerPrefs.GetFloat("combatDescriptorSpeed")); //velocidad minima es demasiado lento
        TextLines.Remove(textLine);
        //Debug.Log("SAKUJO");
    }

    //Limpia el descriptor, eliminando tanto l�neas temporales como PERMANENTES.
    public void Clear()
    {
        TextLines.Clear();
    }

    void RemoveLine(TextLine textLine)
    {
        TextLines.Remove(textLine);
    }
}
