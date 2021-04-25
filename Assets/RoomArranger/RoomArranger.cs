using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArranger : MonoBehaviour
{
    public static RoomArranger instance;

    public GameObject roomPrefab;
    public Vector3 translationBetweenDoors;
    public Vector3 rotationBetweenDoors;
    public Vector3 prologPosition;

    private GameObject previousRoomObject;
    private GameObject currentRoomObject;
    private GameObject nextRoomObject;

    public RoomInterface previousRoom;
    public RoomInterface currentRoom;
    public RoomInterface nextRoom;

    void Start()
    {
        RoomArranger.instance = this;

        // Create the prolog.
        currentRoomObject = Instantiate(roomPrefab, prologPosition, Quaternion.identity);
        currentRoom = currentRoomObject.GetComponent<RoomInterface>();
        currentRoom.Arrange(RoomIteration.Prolog);

        // Create room One.
        nextRoomObject = Instantiate(roomPrefab, prologPosition, Quaternion.identity);
        nextRoom = nextRoomObject.GetComponent<RoomInterface>();
    }

    void Update()
    {

    }

    public void OnDoorOpened()
    {
        Debug.Log("OnDoorOpened");

        // TODO
    }

    public void OnDoorClosed()
    {
        Debug.Log("OnDoorClosed");

        Destroy(previousRoomObject);
        previousRoomObject = null;
        previousRoom = null;
    }

    public void OnRoomEntered()
    {
        RoomIteration enteredIteration = currentRoom.isCompleted ? GetNextRoomIteration(currentRoom.iteration) : currentRoom.iteration;

        Debug.Log("OnRoomEntered: " + enteredIteration);

        previousRoomObject = currentRoomObject;
        currentRoomObject = nextRoomObject;

        previousRoom = currentRoom;
        currentRoom = nextRoom;

        // Create next room.
        nextRoomObject = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
        Quaternion nextRoomRotation = enteredIteration != RoomIteration.Epilog ? currentRoomObject.transform.rotation * Quaternion.Euler(rotationBetweenDoors) : currentRoomObject.transform.rotation;
        Vector3 nextRoomPosition = enteredIteration != RoomIteration.Epilog ? currentRoomObject.transform.position + translationBetweenDoors : currentRoomObject.transform.position;
        nextRoomObject.transform.rotation = nextRoomRotation;
        nextRoomObject.transform.position = nextRoomPosition;
        nextRoom = nextRoomObject.GetComponent<RoomInterface>();

        // Arrange the current room.
        currentRoom.Arrange(enteredIteration);
    }

    public void OnRoomCompleted()
    {
        Debug.Log("OnRoomCompleted: " + currentRoom.iteration);

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
