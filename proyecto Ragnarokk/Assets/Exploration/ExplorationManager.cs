using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager : Singleton<ExplorationManager>
{
    public Floor testFloor;


    public Room RoomPrefab;

    private Vector2Int currentRoomCoords = new Vector2Int();
    private Room currentRoom;


    private Vector2 roomOffset = new Vector2(-5,-5);
    private Vector2 roomSize = new Vector2(1, 1);

    public Floor CurrentFloorData { get; private set; }
    public bool FloorIsLoaded { get; private set; }

    private List<Room> LoadedRooms = new List<Room>();


    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }

    private void UnloadFloor()
    {
        foreach (var room in LoadedRooms)
        {
            Destroy(room.gameObject);
        }
        LoadedRooms.Clear();

        FloorIsLoaded = false;
        CurrentFloorData = null;
    }

    public void InitFloor(Floor floorToLoad)
    {

        UnloadFloor();
        Debug.Log("Loading floor");
        

        int lenghtWidth = floorToLoad.RoomLayout.GetLength(0);
        int lenghtHeight = floorToLoad.RoomLayout.GetLength(1);

        Room[,] tempRoomCollection = new Room[lenghtWidth, lenghtHeight];

        //se llena la coleccion temporal, para asi saber cual sala es vecina de cual.
        for (int y = 0; y < lenghtHeight; y++)
        {
            for (int x = 0; x < lenghtWidth; x++)
            {
                var roomData = floorToLoad.RoomLayout[x, y];
                if (roomData != null)
                {
                    var roomInstance = Instantiate(RoomPrefab, this.transform);
                    tempRoomCollection[x, y] = roomInstance;
                }

            }
        }
        //ahora se obtienen los vecinos y se inicializan las salas posteriormente.
        for (int y = 0; y < lenghtHeight; y++)
        {
            for (int x = 0; x < lenghtWidth; x++)
            {
                var currRoom = tempRoomCollection[x, y];
                if (currRoom != null)
                {
                    //primero encuentro los vecinos.
                    var neightbours = new List<Room>();
                    //se ve si a la izqierda hay un vecino.
                    if (x > 0)
                    {
                        if (tempRoomCollection[x - 1, y] != null)
                        {
                            neightbours.Add(tempRoomCollection[x - 1, y]);
                        }
                    }
                    //se ve si a la derecha hay un vecino.
                    if (x < lenghtWidth - 1)
                    {
                        if (tempRoomCollection[x + 1, y] != null)
                        {
                            neightbours.Add(tempRoomCollection[x + 1, y]);
                        }
                    }
                    //se ve si arriba hay un vecino.
                    if (y > 0)
                    {
                        if (tempRoomCollection[x, y - 1] != null)
                        {
                            neightbours.Add(tempRoomCollection[x, y - 1]);
                        }
                    }
                    //se ve si abajo hay un vecino.
                    if (y < lenghtHeight - 1)
                    {
                        if (tempRoomCollection[x, y + 1] != null)
                        {
                            neightbours.Add(tempRoomCollection[x, y + 1]);
                        }
                    }


                    //Ahora inicializo la room.
                    currRoom.Init(floorToLoad.RoomLayout[x, y], neightbours, new Vector2Int(x,y));
                    currRoom.transform.position = new Vector3(roomOffset.x + x * roomSize.x, roomOffset.y + y * roomSize.y);
                    LoadedRooms.Add(currRoom);
                }
            }
        }


        

        CurrentFloorData = floorToLoad;
        FloorIsLoaded = true;
        //cada espacio es de 1 unidsad de mundo.

    }

    

    //private void Awake()
    //{
    //    base.OnAwake();
    //}

    // Start is called before the first frame update
    void Start()
    {

        //InitFloor(testFloor);

    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.GameState == GAME_STATE.EXPLORATION)
        {

            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            int childCount = transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        //if (GameManager.Instance.CurrentFloor != null && !FloorIsLoaded)
        //{
            
        //    InitFloor(GameManager.Instance.CurrentFloor);
        //    //GameManager.Instance.FloorNeedToBeLoaded = false;
        //}
    }
}
