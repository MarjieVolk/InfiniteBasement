using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** A trigger that activates when the player exits the room, up or down. */
public class ExitRoomTrigger : CollisionTrigger
{
    [Tooltip("Set to false if this corresponds to the player walking downstairs.")]
    public bool isUpperDoorway = true;

    [Tooltip("This indicates whether this trigger is used to detect when the player backtracks"
        + " through the same door they came in from. Otherwise, we disable the normal exit"
        + " trigger on that door.")]
    public bool isGoBackTrigger = false;

    [Tooltip("The direction pointing outward from the doorway, generally away from the player.")]
    public Vector3 outwardNormal;

    void Start()
    {
        triggerOnlyOnce = false;
    }

    override protected void OnTrigger()
    {
        bool isFalsePositiveWithExitFromTeleporting =
            !isGoBackTrigger && PlayerController.instance.lastExitedUpperDoor != isUpperDoorway;
        // TODO: Remove.
        //Debug.Log("ExitRoomTrigger.OnTrigger: " +
        //    "isFalsePositiveWithExitFromTeleporting=" + isFalsePositiveWithExitFromTeleporting +
        //    ", isUpperDoorway=" + isUpperDoorway +
        //    ", isGoBackTrigger=" + isGoBackTrigger +
        //    ", lastExitedUpperDoor=" + PlayerController.instance.lastExitedUpperDoor +
        //    ", hasMovedSinceTeleporting=" + PlayerController.instance.hasMovedSinceTeleporting);
        if (!PlayerController.instance.hasMovedSinceTeleporting || isFalsePositiveWithExitFromTeleporting)
        {
            return;
        }
        base.OnTrigger();
        Vector3 displacementPastDoor = PlayerController.instance.transform.position + PlayerController.instance.GetRadius() * outwardNormal - transform.position;
        displacementPastDoor.y = 0;
        
        // Ensure all doors are open so we don't collide during teleportation.
        (RoomArranger.instance.currentRoom as Room).SetDoorOpen(true, true);
        (RoomArranger.instance.currentRoom as Room).SetDoorOpen(true, false);

        // TODO: Undo this after fixing teleportation
        displacementPastDoor += outwardNormal * 0.04f;

        PlayerController.instance.OnRoomExited(isUpperDoorway, displacementPastDoor);
    }
}
