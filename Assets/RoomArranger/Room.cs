using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    Animator[] roomAnimators;
    Animator[] bigCurtains;

    MusicSwitcher musicSwitcher;

    static bool hasStartDoorEverBeenClosed = false;

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

        roomAnimators = GameObject.FindGameObjectsWithTag("RoomAnimator").Select(x => x.GetComponent<Animator>()).ToArray();
        bigCurtains = GameObject.FindGameObjectsWithTag("Curtains").Select(x => x.GetComponent<Animator>()).ToArray();

        UnhighlightAllInteractableObjects();
        DisableAllTriggers();

        foreach (Renderer renderer in GameObject.FindObjectsOfType<Renderer>())
        {
            renderer.material.SetFloat(highlightHideShaderPropertyName, highlightDisabledValue);
        }
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

    override public void OnTrigger(Triggers triggerType)
    {
        switch (triggerType)
        {
            case Triggers.EndDoor:
                PlayerController.instance.FadeToMenu();
                if (AmbientNoiseSwitcher.instance != null)
                {
                    AmbientNoiseSwitcher.instance.PlayBackgroundNoise(BackgroundNoise.Outdoors);
                }
                break;

            case Triggers.OpenTheStartDoor:
                Debug.Log("foo");
                SetDoorOpen(true, true);
                break;

            case Triggers.Window:
                foreach (Animator curtains in bigCurtains)
                {
                    curtains.SetTrigger("curtainsOpen");
                }

                foreach (Animator anim in roomAnimators)
                {
                    anim.SetTrigger("loop1_clear");
                }

                EnableTriggersOfType(Triggers.Gramophone);
                break;

            case Triggers.Gramophone:
                if (iteration == RoomIteration.One)
                {
                    musicSwitcher.PlayMusic(Music.Piano1);
                    PlayerController.instance.MarkRoomAsCompleted();
                }
                break;

            case Triggers.Sponge:
                EnableTriggersOfType(Triggers.Picture3);
                break;

            case Triggers.Picture3:
                EnableTriggersOfType(Triggers.Picture1);
                Destroy(GetObjectForTrigger(Triggers.Sponge));
                Destroy(GetTriggerObjectForTrigger(Triggers.Sponge));
                break;

            case Triggers.Picture1:
                EnableTriggersOfType(Triggers.Picture2);
                break;

            case Triggers.Picture2:
                // TODO: Remove me after setting up room completion here.
                if (iteration == RoomIteration.Two)
                {
                    PlayerController.instance.MarkRoomAsCompleted();
                }
                break;
            case Triggers.Phone:
                SetTriggerIsActive(Triggers.EndDoor, true);
                EnableTriggersOfType(Triggers.EndDoor);

                //SetDoorOpen(false, false);

                // TODO: Theoretically SetDoorOpen should make the exit door be closed,
                // but it does not.  This is a replacement for that.  I made a separate 
                // ClosedEditDoor object in the scene which I am turning on.
                GameObject doorObject = GetObjectForTrigger(Triggers.EndDoor);
                if (doorObject != null)
                {
                    doorObject.GetComponent<MeshRenderer>().enabled = true;
                    doorObject.GetComponent<BoxCollider>().enabled = true;
                }
                break;
            default:
                break;
        }
    }

    override protected void ArrangeForRoomOne()
    {

        if (!hasStartDoorEverBeenClosed)
        {
            hasStartDoorEverBeenClosed = true;
            SetDoorOpen(false, true);
            SetTriggerIsActive(Triggers.StartDoor, true);
        }
        else
        {
            // If the start door is open, disable its trigger
            SetTriggerIsActive(Triggers.StartDoor, false);
        }

        EnableTriggersOfType(Triggers.Unknown);
        EnableTriggersOfType(Triggers.Window);

        if (AmbientNoiseSwitcher.instance != null)
        {
            AmbientNoiseSwitcher.instance.PlayBackgroundNoise(BackgroundNoise.Basement);
        }
    }

    override protected void ArrangeForRoomTwo()
    {
        musicSwitcher.PlayMusic(Music.Piano2);
        EnableTriggersOfType(Triggers.Sponge);
        foreach (Animator anim in roomAnimators)
        {
            anim.GetComponent<Animator>().SetTrigger("loop2_clear");
        }
    }

    override protected void ArrangeForRoomThree()
    {
        musicSwitcher.PlayMusic(Music.Piano3);
        EnableTriggersOfType(Triggers.Phone);

        foreach (Animator anim in roomAnimators)
        {
            anim.GetComponent<Animator>().SetTrigger("loop3_clear");
        }
    }

    public void SetDoorOpen(bool isOpen, bool isUpperDoor)
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


    override protected void ArrangeForRoomFour()
    {
        // Unused.
    }

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
