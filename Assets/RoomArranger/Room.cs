using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : RoomInterface
{
    public float doorOpenRotationY = -91.68f;
    public float doorClosedRotationY = 0;

    GameObject stairwellContents;
    GameObject interactableObjectsContainer;
    GameObject furnitureContainer;
    GameObject houseContainer;


    GameObject upperRoomCopyStairwellContents;
    GameObject lowerRoomCopyStairwellContents;
    GameObject lowerCopyRoomFurnitureContainer;

    void Start()
    {
        stairwellContents = GameObject.Find("StairwellContents");
        interactableObjectsContainer = stairwellContents.transform.Find("InteractableObjects").gameObject;
        furnitureContainer = stairwellContents.transform.Find("Furniture").gameObject;
        houseContainer = stairwellContents.transform.Find("House").gameObject;

        upperRoomCopyStairwellContents = GameObject.Find("UpperRoomCopyStairwellContents");
        lowerRoomCopyStairwellContents = GameObject.Find("LowerRoomCopyStairwellContents");
        lowerCopyRoomFurnitureContainer = lowerRoomCopyStairwellContents != null ? lowerRoomCopyStairwellContents.transform.Find("Furniture").gameObject : null;

        UnhighlightAllInteractableObjects();
    }

    override public GameObject GetObjectForTrigger(Triggers triggerType)
    {
        if (triggerToObjectName.ContainsKey(triggerType))
        {
            Transform objectTransform = interactableObjectsContainer.transform.Find(triggerToObjectName[triggerType]);
            if (objectTransform != null)
            {
                return objectTransform.gameObject;
            }
            else
            {
                Debug.LogError("Object for trigger not found: trigger=" + triggerType + ", objectName=" + triggerToObjectName[triggerType]);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No object name mapped for trigger type: " + triggerType);
            return null;
        }
    }

    override protected void UnhighlightAllInteractableObjects()
    {
        foreach (Triggers triggerType in triggerToObjectName.Keys)
        {
            string identifier = triggerToObjectName[triggerType];
            Transform objectTransform = interactableObjectsContainer.transform.Find(identifier);
            if (objectTransform != null)
            {
                SetObjectHighlight(interactableObjectsContainer.transform.Find(identifier).gameObject, false);
            }
            else
            {
                Debug.LogError("Object for trigger not found: trigger=" + triggerType + ", objectName=" + identifier);
            }
        }
    }

    override public void OnTrigger(Triggers triggerType)
    {
        switch (triggerType)
        {
            case Triggers.EndDoor:
                QuitToMenu();
                break;
            
            // TODO: React to individual triggers as needed.

            default:
                break;
        }
    }

    override protected void ArrangeForRoomOne()
    {
        SetDoorOpen(false, true);
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
        SetDoorOpen(false, false);
    }

    void SetDoorOpen(bool isOpen, bool isUpperDoor)
    {
        GameObject container = isUpperDoor ? furnitureContainer : lowerCopyRoomFurnitureContainer;
        if (container == null)
        {
            Debug.LogWarning("Trying to open/close door for non-existent room copy: isUpperDoor=" + isUpperDoor);
            return;
        }
        float rotationY = isOpen ? doorOpenRotationY : doorClosedRotationY;
        container.transform.Find("Door_1").gameObject.transform.rotation = Quaternion.Euler(0, rotationY, 0);
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
