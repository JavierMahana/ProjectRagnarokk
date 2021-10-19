using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(SpriteRenderer))]
public class Room : MonoBehaviour
{

    public bool IsInitialized => data != null;

    private List<Room> adyacentRooms = new List<Room>();

    public bool Visitable = false;
    public bool Cleared = false;

    private RoomData data;
    //ahora mismo los estados se van a representar mediante el cambio de color.
    //lo ideal es que luego se representen mediante el cambio de sprite
    private Color unknownColor = new Color(0.2f, 0.2f, 0.2f, 1);
    private Color clearedColor = Color.gray;
    private Color visitableColor = Color.white;
    private Color highlithedColor = Color.green;

    private SpriteRenderer spriteRenderer;
    private bool onMouseHover;

    public void Init(RoomData data, List<Room> adyacentRooms)
    {
        this.data = data;
        this.adyacentRooms = adyacentRooms;

        if (data.FloorStart)
        {
            Cleared = true;
            foreach (var neightbor in adyacentRooms)
            {
                neightbor.Visitable = true;
            }
        }
    }

    private void OnMouseEnter()
    {
        onMouseHover = true;
    }
    private void OnMouseExit()
    {
        onMouseHover = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("Click!");
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.grey;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Cleared)
        {
            spriteRenderer.color = clearedColor;
            spriteRenderer.sprite = data.VisibleSprite;
        }
        else if (Visitable)
        {
            if (onMouseHover)
            {
                spriteRenderer.color = highlithedColor;
            }
            else
            {
                spriteRenderer.color = visitableColor;
            }
            spriteRenderer.sprite = data.VisibleSprite;
        }
        else
        {
            spriteRenderer.color = unknownColor;
            spriteRenderer.sprite = ExplorationManager.Instance.CurrentFloorData.NotVisibleSprite;
        }
    }
}
