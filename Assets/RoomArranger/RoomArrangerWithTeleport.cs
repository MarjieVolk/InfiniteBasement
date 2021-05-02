using UnityEngine;

public class RoomArrangerWithTeleport : RoomArranger
{
    public static void GetTeleportTransform(bool isUpperDoorway, Vector3 displacementPastDoor, out Vector3 rotation, out Vector3 translation)
    {
        Vector3 offsetForDisplacementPastDoor = Quaternion.Euler(instance.rotationBetweenDoors) * displacementPastDoor;
        if (isUpperDoorway)
        {
            rotation = -instance.rotationBetweenDoors;
            translation = instance.translationBetweenDoors + offsetForDisplacementPastDoor;
        }
        else
        {
            rotation = instance.rotationBetweenDoors;
            translation = -instance.translationBetweenDoors + offsetForDisplacementPastDoor;
        }
    }

    override protected void UpdateForNewRoom(RoomIteration enteredIteration, bool isUpperDoorway, Vector3 displacementPastDoor)
    {
        // Teleport player.
        Vector3 rotation;
        Vector3 translation;
        if (enteredIteration != RoomIteration.Epilog)
        {
            GetTeleportTransform(isUpperDoorway, displacementPastDoor, out rotation, out translation);
        }
        else
        {
            rotation = Vector3.zero;
            translation = Vector3.zero;
        }

        Vector3 positionEnd = PlayerController.instance.transform.position + translation;

        Debug.Log("Teleporting player: " +
            "fromUpperDoorway=" + isUpperDoorway +
            ", enteringIteration=" + enteredIteration +
            ", positionStart=" + PlayerController.instance.transform.position +
            ", positionEnd=" + positionEnd +
            ", rotationStart=" + PlayerController.instance.transform.rotation +
            ", rotation=" + rotation);

        PlayerController.instance.Teleport(translation, rotation);
    }
}
