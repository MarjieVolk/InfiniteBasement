using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using TimeSpan = System.TimeSpan;

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

    Animator[] bigCurtains;

    MusicSwitcher musicSwitcher;
    LightSwitcher lightSwitcher;

    static bool hasStartDoorEverBeenClosed = false;

    void Start()
    {
        stairwellContents = GameObject.Find("StairwellContents");
        interactableObjectsContainer = stairwellContents.transform.Find("InteractableObjects").gameObject;
        furnitureContainer = stairwellContents.transform.Find("Furniture").gameObject;
        houseContainer = stairwellContents.transform.Find("House").gameObject;
        musicSwitcher = GameObject.Find("Ambient Sound").transform.GetComponent<MusicSwitcher>();
        lightSwitcher = GameObject.Find("RoomArrangerWithTeleport").GetComponent<LightSwitcher>();

        upperRoomCopyStairwellContents = GameObject.Find("DummyRoomTop");
        lowerRoomCopyStairwellContents = GameObject.Find("DummyRoomBottom");
        lowerCopyRoomFurnitureContainer = lowerRoomCopyStairwellContents != null ? lowerRoomCopyStairwellContents.transform.Find("Furniture").gameObject : null;

        triggerContainer = GameObject.Find("Gameplay");

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

    int cleanedPictureCount = 0;

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
                SetDoorOpen(true, true);
                break;
            case Triggers.MaybeOpenTheEndDoor:
                if (iteration == RoomIteration.Three && isCompleted)
                {
                    SetDoorOpen(true, false);
                }
                break;
            case Triggers.MaybeCloseTheEndDoor:
                Debug.Log("Triggers.MaybeCloseTheEndDoor: iteration=" + iteration + ", isCompleted=" + isCompleted);
                if (iteration == RoomIteration.Three && isCompleted)
                {
                    SetDoorOpen(false, false);
                }
                break;

            case Triggers.Window:
                TriggerAnimation("curtainsOpen", bigCurtains);
                lightSwitcher.IncrementLight();
                EnableTriggersOfType(Triggers.Gramophone);
                break;

            case Triggers.Gramophone:
                if (iteration == RoomIteration.One)
                {
                    OnRoomCompleted();
                }
                break;

            case Triggers.Sponge:
                EnableTriggersOfType(Triggers.Picture1);
                EnableTriggersOfType(Triggers.Picture2);
                EnableTriggersOfType(Triggers.Picture3);
                // Hide the sponge.
                GetObjectForTrigger(Triggers.Sponge).SetActive(false);
                // Deactivate the trigger after the voice-over finishes playing.
                Destroy(GetTriggerObjectForTrigger(triggerType), 1.8f);
                break;

            case Triggers.Picture1:
            case Triggers.Picture2:
            case Triggers.Picture3:
                OnPictureCleaned();
                break;

            case Triggers.Phone:
                OnRoomCompleted();
                break;

            default:
                break;
        }
    }

    void OnPictureCleaned()
    {
        cleanedPictureCount++;
        if (iteration == RoomIteration.Two && cleanedPictureCount == 3 && !isCompleted)
        {
            OnRoomCompleted();
        }
    }

    override protected void OnRoomCompleted()
    {
        Debug.Log("** OnRoomCompleted: " + iteration);

        isCompleted = true;

        musicSwitcher.IncrementMusic();

        switch (iteration)
        {
            case RoomIteration.One:
                break;
            case RoomIteration.Two:
                break;
            case RoomIteration.Three:
                SetDoorOpen(false, false);
                break;
            case RoomIteration.Four:
                break;
            default:
                Debug.LogError("Unrecognized room iteration: " + iteration);
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
        EnableTriggersOfType(Triggers.Sponge);
        lightSwitcher.IncrementLight();
    }

    override protected void ArrangeForRoomThree()
    {
        AudioSource phoneAudioSource = GetTriggerObjectForTrigger(Triggers.Phone).GetComponent<AudioSource>();
        phoneAudioSource.clip = RoomArranger.instance.phoneRingClip;
        phoneAudioSource.Play();

        EnableTriggersOfType(Triggers.Phone);
        lightSwitcher.IncrementLight();
    }

    public void SetDoorOpen(bool isOpen, bool isUpperDoor)
    {
        Debug.Log("Room.SetDoorOpen: isOpen=" + isOpen + ", isUpperDoor=" + isUpperDoor);

        if (!isUpperDoor)
        {
            GameObject doorObject = GetObjectForTrigger(Triggers.EndDoor);
            if (doorObject != null)
            {
                doorObject.GetComponent<MeshRenderer>().enabled = !isOpen;
                doorObject.GetComponent<BoxCollider>().enabled = !isOpen;
            }
            if (!isOpen)
            {
                SetTriggerIsActive(Triggers.EndDoor, true);
                EnableTriggersOfType(Triggers.EndDoor);
            }
        }
        else
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
    }

    public void TriggerAnimation(string animationName, Animator[] animatorCopies)
    {
        foreach (Animator animator in animatorCopies)
        {
            animator.SetTrigger(animationName);
        }
    }

    // --- Unused stuff --- //

    override protected void ArrangeForRoomFour()
    {

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
