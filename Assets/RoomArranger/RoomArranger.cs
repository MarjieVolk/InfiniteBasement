using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArranger : MonoBehaviour
{
    public static RoomArranger instance;

    public GameObject roomPrefab;
    public Vector3 displacementBetweenDoors;

    private GameObject previousRoomObject;
    private GameObject currentRoomObject;
    private GameObject nextRoomObject;

    public Room previousRoom;
    public Room currentRoom;
    public Room nextRoom;

    void Start()
    {
        RoomArranger.instance = this;

        // Create the prolog.
        Vector3 currentRoomPosition = Vector3.zero;
        currentRoomObject = Instantiate(roomPrefab, currentRoomPosition, Quaternion.identity);
        currentRoom = currentRoomObject.GetComponent<Room>();
        currentRoom.Arrange(RoomIteration.Prolog);

        // Create room One.
        Vector3 nextRoomPosition = currentRoomObject.transform.position + displacementBetweenDoors;
        nextRoomObject = Instantiate(roomPrefab, nextRoomPosition, Quaternion.identity);
        nextRoom = nextRoomObject.GetComponent<Room>();
        nextRoom.Arrange(RoomIteration.One);
    }

    void Update()
    {

    }

    public void OnDoorOpened()
    {
        // TODO
    }

    public void OnDoorClosed()
    {
        Destroy(previousRoomObject);
        previousRoomObject = null;
        previousRoom = null;
    }

    public void OnRoomEntered()
    {
        RoomIteration enteredIteration = currentRoom.isCompleted ? GetNextRoomIteration(currentRoom.iteration) : currentRoom.iteration;

        previousRoomObject = currentRoomObject;
        currentRoomObject = nextRoomObject;

        previousRoom = currentRoom;
        currentRoom = nextRoom;

        // Create next room.
        Vector3 nextRoomPosition = currentRoomObject.transform.position + displacementBetweenDoors;
        nextRoomObject = Instantiate(roomPrefab, nextRoomPosition, Quaternion.identity);
        nextRoom = nextRoomObject.GetComponent<Room>();

        // Arrange the current room.
        currentRoom.Arrange(enteredIteration);
    }

    public void OnRoomCompleted()
    {
        currentRoom.isCompleted = true;
    }

    public static RoomIteration GetNextRoomIteration(RoomIteration previousIteration)
    {
        switch (previousIteration)
        {
            case RoomIteration.Prolog:
                return RoomIteration.One;
            case RoomIteration.One:
                return RoomIteration.Two;
            case RoomIteration.Two:
                return RoomIteration.Three;
            case RoomIteration.Three:
                return RoomIteration.Four;
            case RoomIteration.Four:
                return RoomIteration.Five;
            case RoomIteration.Five:
                return RoomIteration.Epilog;
            case RoomIteration.Epilog:
                Debug.LogError("Attempting to leave RoomIteration.Epilog");
                return RoomIteration.Prolog;
            default:
                Debug.LogError("Unrecognized RoomIteration: " + previousIteration);
                return RoomIteration.Prolog;
        }
    }
}
