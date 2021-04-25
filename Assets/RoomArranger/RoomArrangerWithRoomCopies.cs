using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArrangerWithRoomCopies : RoomArranger
{
    protected GameObject previousRoomObject;
    protected GameObject nextRoomObject;

    protected RoomInterface previousRoom;
    protected RoomInterface nextRoom;

    override protected void Start()
    {
        base.Start();

        // Create room One.
        nextRoomObject = Instantiate(roomPrefab, prologPosition, Quaternion.identity);
        nextRoom = nextRoomObject.GetComponent<RoomInterface>();
    }

    // TODO: Replace this with logic to destroy previous previous on enter. Also handle next next.
    public void OnDoorClosed()
    {
        Debug.Log("OnDoorClosed");

        Destroy(previousRoomObject);
        previousRoomObject = null;
        previousRoom = null;
    }

    override protected void UpdateForNewRoom(RoomIteration enteredIteration)
    {
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
    }
}
