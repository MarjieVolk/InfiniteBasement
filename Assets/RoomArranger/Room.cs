using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : RoomInterface
{
    GameObject interactableObjectsContainer;
    GameObject furnitureContainer;
    GameObject houseContainer;

    void Start()
    {
        interactableObjectsContainer = transform.Find("InteractableObjects").gameObject;
        furnitureContainer = transform.Find("Furniture").gameObject;
        houseContainer = transform.Find("House").gameObject;
    }

    override public GameObject GetObjectForTrigger(Triggers triggerType)
    {
        string objectName;
        switch (triggerType)
        {
            case Triggers.Gramaphone:
                objectName = "Gramaphone01";
                break;
            case Triggers.Sponge:
                objectName = "SpongeForCleaning";
                break;
            case Triggers.Picture1:
                objectName = "Picture_1";
                break;
            case Triggers.Picture2:
                objectName = "Picture_2";
                break;
            case Triggers.Picture3:
                objectName = "Picture_3";
                break;
            case Triggers.Phone:
                objectName = "Phone";
                break;
            default:
                Debug.LogError("Unrecognized triggerType: " + triggerType);
                return null;
        }
        return interactableObjectsContainer.transform.Find(objectName).gameObject;
    }

    override public void OnTrigger(Triggers triggerType)
    {
        switch (triggerType)
        {
            // TODO: React to individual triggers.
            default:
                break;
        }
    }

    override protected GameObject GetReferenceToPrologContainer()
    {
        return transform.Find("Prolog").gameObject;
    }

    override protected GameObject GetReferenceToEpilogContainer()
    {
        return transform.Find("Epilog").gameObject;
    }

    override protected GameObject GetReferenceToMainRoomContainer()
    {
        return transform.Find("MainRoom").gameObject;
    }

    override protected void ArrangeForRoomOne()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
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

    override protected void ArrangeForRoomFive()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForProlog()
    {
        isCompleted = true;
        // TODO: Show/hide/move/adjust whatever items/state is needed for the prolog.
    }

    override protected void ArrangeForEpilog()
    {
        isCompleted = true;
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }
}
