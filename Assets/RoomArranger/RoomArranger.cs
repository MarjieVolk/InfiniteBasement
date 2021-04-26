using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class RoomArranger : MonoBehaviour
{
    public static RoomArranger instance;

    public GameObject roomPrefab;
    public Vector3 translationBetweenDoors;
    public Vector3 rotationBetweenDoors;
    public Vector3 startPosition;

    protected GameObject currentRoomObject;
    public RoomInterface currentRoom;

    virtual protected void Start()
    {
        RoomArranger.instance = this;

        // Create the first room.
        currentRoomObject = Instantiate(roomPrefab, startPosition, Quaternion.identity);
        currentRoom = currentRoomObject.GetComponent<RoomInterface>();

        StartCoroutine(TriggerDelayedStart());
    }

    IEnumerator TriggerDelayedStart()
    {
        yield return 0;
        DelayedStart();
    }

    virtual protected void DelayedStart()
    {
        currentRoom.Arrange(RoomIteration.One);
    }

    public void OnRoomExited(bool isUpperDoorway, Vector3 displacementPastDoor)
    {
        RoomIteration previousIteration = currentRoom.iteration;
        RoomIteration nextIteration;
        if (!isUpperDoorway && currentRoom.isCompleted)
        {
            nextIteration = GetNextRoomIteration(previousIteration);
        }
        else
        {
            nextIteration = previousIteration;
        }

        Debug.Log("OnRoomExited: " +
            "from=" + previousIteration + 
            ", to=" + nextIteration + 
            ", isUpperDoorway=" + isUpperDoorway + 
            ", displacementPastDoor=" + displacementPastDoor);

        UpdateForNewRoom(nextIteration, isUpperDoorway, displacementPastDoor);

        // Arrange the current room.
        currentRoom.isCompleted = false;
        currentRoom.Arrange(nextIteration);
    }

    abstract protected void UpdateForNewRoom(RoomIteration enteredIteration, bool isUpperDoorway, Vector3 displacementPastDoor);

    public void OnRoomCompleted()
    {
        Debug.Log("OnRoomCompleted: " + currentRoom.iteration);

        currentRoom.isCompleted = true;
    }

    public void OnTrigger(Triggers triggerType, bool isEnabled)
    {
        currentRoom.OnTrigger(triggerType, isEnabled);
    }

    public void OnTriggerTargetChange(Triggers triggerType, bool isTargeted)
    {
        currentRoom.OnTriggerTargetChange(triggerType, isTargeted);
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
                return RoomIteration.Unknown;
            case RoomIteration.Unknown:
            default:
                Debug.LogError("Unrecognized RoomIteration: " + previousIteration);
                return RoomIteration.Unknown;
        }
    }
}
