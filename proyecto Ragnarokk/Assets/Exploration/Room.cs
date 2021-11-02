using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider), typeof(SpriteRenderer))]
public class Room : MonoBehaviour
{



    public bool IsInitialized => data != null;

    private List<Room> adyacentRooms = new List<Room>();
    public Vector2Int Coordenates { get; private set; }

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

    public void Init(RoomData data, List<Room> adyacentRooms, Vector2Int coordenates)
    {
        this.data = data;
        this.adyacentRooms = adyacentRooms;
        Coordenates = coordenates;

        if (data.FloorStart)
        {
            MarkAsCleared();
        }
    }
    public void MarkAsCurrent()
    {
        ExplorationManager.Instance.SetCurrentRoom(this);
    }
    public void MarkAsCleared()
    {
        Cleared = true;
        foreach (var neightbor in adyacentRooms)
        {
            if (!neightbor.Cleared)
            {
                neightbor.Visitable = true;
            }
        }
    }

    public void TryActivateLorePanel(Sprite sprite, string text)
    {
        var lorePanel = FindObjectOfType<LorePanel>(true);
        if (lorePanel != null)
        {
            lorePanel.UpdateContent(sprite, text);
            lorePanel.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No hay ningun lore panel en la escena! no se puede activar.");
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
        if (Visitable)
        {
            data.LoadRoom(GameManager.Instance, SceneChanger.Instance, this);
        }
        //Debug.Log("Click!");
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
