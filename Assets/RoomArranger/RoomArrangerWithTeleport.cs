using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomArrangerWithTeleport : RoomArranger
{
    override protected void UpdateForNewRoom(RoomIteration enteredIteration, bool isUpperDoorway, Vector3 displacementPastDoor)
    {
        // Teleport player.
        Vector3 rotation;
        Vector3 translation;
        if (enteredIteration != RoomIteration.Epilog)
        {
            Vector3 offsetForDisplacementPastDoor = Quaternion.Euler(rotationBetweenDoors) * displacementPastDoor;
            if (isUpperDoorway)
            {
                rotation = -rotationBetweenDoors;
                translation = translationBetweenDoors + offsetForDisplacementPastDoor;
            }
            else
            {
                rotation = rotationBetweenDoors;
                translation = -translationBetweenDoors + offsetForDisplacementPastDoor;
            }
        }
        else
        {
            rotation = Vector3.zero;
            translation = Vector3.zero;
        }

        Debug.Log("Teleporting player: " +
            "fromUpperDoorway=" + isUpperDoorway +
            ", enteringIteration=" + enteredIteration +
            ", positionStart=" + PlayerController.instance.transform.position +
            ", translation=" + translation +
            ", rotationStart=" + PlayerController.instance.transform.rotation +
            ", rotation=" + rotation);

        PlayerController.instance.Teleport(translation, rotation);
    }
}
