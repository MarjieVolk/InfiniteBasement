using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArrangerWithTeleport : RoomArranger
{
    override protected void UpdateForNewRoom(RoomIteration enteredIteration)
    {
        // Teleport player.
        Quaternion nextRoomRotation = enteredIteration != RoomIteration.Epilog ? currentRoomObject.transform.rotation * Quaternion.Euler(rotationBetweenDoors) : currentRoomObject.transform.rotation;
        Vector3 nextRoomPosition = enteredIteration != RoomIteration.Epilog ? currentRoomObject.transform.position + translationBetweenDoors : currentRoomObject.transform.position;
        PlayerController.instance.transform.rotation = nextRoomRotation;
        PlayerController.instance.transform.position = nextRoomPosition;
    }
}
