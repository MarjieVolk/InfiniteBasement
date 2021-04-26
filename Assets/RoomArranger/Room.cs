using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : RoomInterface
{
    public float doorOpenRotationY = -91.68f;
    public float doorClosedRotationY = 0;

    private GameObject interactableObjectsContainer;
    private GameObject furnitureContainer;
    private GameObject houseContainer;

    void Start()
    {
        interactableObjectsContainer = GameObject.Find("InteractableObjects");
        furnitureContainer = GameObject.Find("Furniture");
        houseContainer = GameObject.Find("House");
    }

    override public void OnTrigger(Triggers triggerType)
    {
        switch (triggerType)
        {
            // TODO: React to individual triggers.
            case Triggers.StartDoor:
                break;
            case Triggers.EndDoor:
                QuitToMenu();
                break;
            default:
                break;
        }
    }

    override protected void ArrangeForRoomOne()
    {
        // TODO: Consider closing the door (at only one entrance).
        //SetDoorOpen(false);
    }

    override protected void ArrangeForRoomTwo()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomThree()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomFour()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    void SetDoorOpen(bool isOpen)
    {
        float rotationY = isOpen ? doorOpenRotationY : doorClosedRotationY;
        furnitureContainer.transform.Find("Door_1").gameObject.transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }


    // --- Unused stuff --- //


    override protected void ArrangeForRoomFive()
    {
        // Unused.
    }

    override protected void ArrangeForProlog()
    {
        // Unused.
        isCompleted = true;
    }

    override protected void ArrangeForEpilog()
    {
        // Unused.
        isCompleted = true;
    }

    override protected GameObject GetReferenceToPrologContainer()
    {
        // Unused.
        return null;
    }

    override protected GameObject GetReferenceToEpilogContainer()
    {
        // Unused.
        return null;
    }

    override protected GameObject GetReferenceToMainRoomContainer()
    {
        // Unused.
        return null;
    }
}
