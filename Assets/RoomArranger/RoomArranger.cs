using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class RoomArranger : MonoBehaviour
{
    public static RoomArranger instance;

    public GameObject roomPrefab;
    public Vector3 translationBetweenDoors;
    public Vector3 rotationBetweenDoors;
    public Vector3 prologPosition;

    protected GameObject currentRoomObject;
    protected RoomInterface currentRoom;

    virtual protected void Start()
    {
        RoomArranger.instance = this;

        // Create the prolog.
        currentRoomObject = Instantiate(roomPrefab, prologPosition, Quaternion.identity);
        currentRoom = currentRoomObject.GetComponent<RoomInterface>();
        currentRoom.Arrange(RoomIteration.Prolog);
    }

    public void OnRoomEntered()
    {
        RoomIteration enteredIteration = currentRoom.isCompleted ? GetNextRoomIteration(currentRoom.iteration) : currentRoom.iteration;

        Debug.Log("OnRoomEntered: " + enteredIteration);

        UpdateForNewRoom(enteredIteration);

        // Arrange the current room.
        currentRoom.Arrange(enteredIteration);
    }

    abstract protected void UpdateForNewRoom(RoomIteration enteredIteration);

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
