using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Clase encargada de manejar la caja de descripción de combate, describiendo gran parte de los sucesos de un combate, según se le indica desde otras
//clases.
public class CombatDescriptor : MonoBehaviour
{
    //Clase de líneas de texto que el descriptor deberá mostrar
    class TextLine
    {
        public string Line { private set; get; } //Texto
        public float PermanenceTime; //Tiempo que la línea permanecerá desde que es la línea principal
        public bool IsFirstLine; //Indica si ha sido declarada como línea principal

        public TextLine(string line, float permTime)
        {
            Line = line;
            PermanenceTime = permTime;
            IsFirstLine = false;
        }
    }

    public GameObject CanvasCombatDescriptor; //Prefab del canvas del descriptor (EN DESUSO)
    public TextMeshProUGUI Text; //Texto único del canvas, el cual muestra las líneas de texto
    List<TextLine> TextLines; //Todas las líneas que serán mostradas
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
            //Actualización continua del texto mostrado, según las líneas de texto almacenadas
            foreach (TextLine textLine in TextLines)
            {
                string n = textLine.Equals(TextLines[0]) ? "" : "\n";
                string line = textLine.Line;
                Text.text = line + n + Text.text; //Las líneas son mostradas desde abajo hacia arriba
            }

            //Cuando la primera línea de texto no ha sido declarada como tal, se hace, e inicia el tiempo de permanencia antes de desaparecer
            //Las líneas permanentes NO se eliminan de esta forma.
            if (TextLines[0].PermanenceTime != 0  &&  !TextLines[0].IsFirstLine)
            {
                TextLines[0].IsFirstLine = true;
                StartCoroutine(RemoveLineDelayed(TextLines[0]));
            }
        }
    }

    //Añade una línea de texto a la lista. No se añadirán strings vacíos, puesto que se generaría un salto de línea innecesario. Se puede
    //especificar un tiempo de permanencia, pero, por defecto, éste es de 1 segundo.
    //Líneas de texto cuyo PermanenceTime es 0 son líneas permanentes, y se requiere limpiar el descriptor para quitarlas. Esto puede
    //generar acumulación de líneas tras una línea permanente si no se gestiona correctamente. Siempre que hayan líneas permanentes
    //mostrándose, se debe limpiar el descriptor con su correspondiente método antes de agregar nuevas líneas temporales.
    public void AddTextLine(string line, float permanenceTime = 1)
    {
        if(line.Equals("")) { return; }
        TextLines.Add(new TextLine(line, permanenceTime));
    }

    //Método especial para mostrar de quién es el turno actual. Limpia el descriptor antes de añadir la línea, la cual es PERMANENTE.
    public void ShowFighterInTurn(Fighter activeFighter, bool isPlayerFighter)
    {
        //Clear();

        string fName = isPlayerFighter? activeFighter.RealName : activeFighter.Name;
        string line = fName + "'s turn";
        TextLines.Add(new TextLine(line, 0));
    }

    //Elimina líneas temporales, esperando el tiempo de permanencia correspondiente desde el momento en que se le declara como primera línea.
    //NO ELIMINA LÍNEAS PERMANENTES
    IEnumerator RemoveLineDelayed(TextLine textLine)
    {
        yield return new WaitForSeconds(textLine.PermanenceTime / PlayerPrefs.GetFloat("combatDescriptorSpeed")); //velocidad minima es demasiado lento
        TextLines.Remove(textLine);
        //Debug.Log("SAKUJO");
    }

    //Limpia el descriptor, eliminando tanto líneas temporales como PERMANENTES.
    public void Clear()
    {
        TextLines.Clear();
    }

    void RemoveLine(TextLine textLine)
    {
        TextLines.Remove(textLine);
    }
}
