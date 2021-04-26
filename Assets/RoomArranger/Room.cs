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

    GameObject triggerContainer;

    GameObject grayRoom;
    GameObject bigCurtains;

    MusicSwitcher musicSwitcher;

    void Start()
    {
        stairwellContents = GameObject.Find("StairwellContents");
        interactableObjectsContainer = stairwellContents.transform.Find("InteractableObjects").gameObject;
        furnitureContainer = stairwellContents.transform.Find("Furniture").gameObject;
        houseContainer = stairwellContents.transform.Find("House").gameObject;
        musicSwitcher = GameObject.Find("Ambient Sound").transform.GetComponent<MusicSwitcher>();

        upperRoomCopyStairwellContents = GameObject.Find("UpperRoomCopyStairwellContents");
        lowerRoomCopyStairwellContents = GameObject.Find("LowerRoomCopyStairwellContents");
        lowerCopyRoomFurnitureContainer = lowerRoomCopyStairwellContents != null ? lowerRoomCopyStairwellContents.transform.Find("Furniture").gameObject : null;

        triggerContainer = GameObject.Find("Gameplay");

        grayRoom = stairwellContents.transform.Find("GrayRoom").gameObject;
        bigCurtains = furnitureContainer.transform.Find("BigCurtains").gameObject;

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
    
    override public GameObject GetTriggerObjectForTrigger(Triggers triggerType)
    {
        if (triggerToTriggerObjectName.ContainsKey(triggerType))
        {
            Transform objectTransform = triggerContainer.transform.Find(triggerToTriggerObjectName[triggerType]);
            if (objectTransform != null)
            {
                return objectTransform.gameObject;
            }
            else
            {
                Debug.LogError("Trigger object for trigger not found: trigger=" + triggerType + ", objectName=" + triggerToTriggerObjectName[triggerType]);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("No trigger object name mapped for trigger type: " + triggerType);
            return null;
        }
    }

    /** Enable all triggers of the given {@code type}. */
    public void EnableTriggersOfType(Triggers type) {
        DefaultTrigger[] defaultTriggers = FindObjectsOfType<DefaultTrigger>();
        foreach (DefaultTrigger trigger in defaultTriggers)
        {
            if (trigger.triggerType == type) {
                trigger.isEnabled = true;
            }
        }
    }
    
    /** Disable all triggers of the given {@code type}. */
    public void DisableTriggersOfType(Triggers type) {
        DefaultTrigger[] defaultTriggers = FindObjectsOfType<DefaultTrigger>();
        foreach (DefaultTrigger trigger in defaultTriggers)
        {
            if (trigger.triggerType == type) {
                trigger.isEnabled = false;
            }
        }
    }

    /** Disable all triggers. */
    public void DisableAllTriggers() {
        DefaultTrigger[] defaultTriggers = FindObjectsOfType<DefaultTrigger>();
        foreach (DefaultTrigger trigger in defaultTriggers)
        {
            trigger.isEnabled = false;
        }
    }

    override protected void UnhighlightAllInteractableObjects()
    {
        foreach (Triggers triggerType in triggerToObjectName.Keys)
        {
            SetObjectHighlight(triggerType, false);
        }
    }

    override protected void DeactivateAllInteractableObjects()
    {
        foreach (Triggers triggerType in triggerToTriggerObjectName.Keys)
        {
            SetTriggerIsActive(triggerType, false);
        }
    }

    override public void OnTrigger(Triggers triggerType, bool isEnabled)
    {
        switch (triggerType)
        {
            case Triggers.EndDoor:
                QuitToMenu();
                break;

            case Triggers.Window:
                bigCurtains.GetComponent<Animator>().SetTrigger("curtainsOpen");
                grayRoom.GetComponent<Animator>().SetTrigger("loop1_clear");
                EnableTriggersOfType(Triggers.Gramophone);
                break;

            case Triggers.Gramophone:
                musicSwitcher.PlayMusic(Music.Piano1);

            default:
                break;
        }
    }

    override protected void ArrangeForRoomOne()
    {
        SetDoorOpen(false, true);
        DisableAllTriggers();
        EnableTriggersOfType(Triggers.Unknown);
        EnableTriggersOfType(Triggers.Window);
        SetTriggerIsActive(Triggers.StartDoor, true);
    }

    override protected void ArrangeForRoomTwo()
    {
        musicSwitcher.PlayMusic(Music.Piano2);
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomThree()
    {
        musicSwitcher.PlayMusic(Music.Piano3);
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomFour()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
        SetDoorOpen(false, false);
        SetTriggerIsActive(Triggers.EndDoor, true);
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
