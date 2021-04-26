using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitRoomTrigger : CollisionTrigger
{
    public bool isUpperDoorway = true;

    // This indicates whether this trigger is used to detect when the player backtracks through the same door they came in from.
    // Otherwise, we disable the normal exit trigger on that door.
    public bool isGoBackTrigger = false;

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
        PlayerController.instance.OnRoomExited(isUpperDoorway, displacementPastDoor);
    }
}
