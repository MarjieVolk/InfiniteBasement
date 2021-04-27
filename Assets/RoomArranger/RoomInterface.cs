using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomIteration
{
    Unknown,

    // CURRENTLY UNUSED. This represents whatever is shown before entering the first door, at the start of the game.
    Prolog,

    One,
    Two,
    Three,
    Four,
    Five,

    // CURRENTLY UNUSED. This represents whatever is shown after leaving the last door, at the end of the game.
    Epilog,
}

public enum Triggers
{
    Unknown,

    StartDoor,
    EndDoor,

    Gramophone,
    Sponge,
    Picture1,
    Picture2,
    Picture3,
    Phone,

    Window,

    // Teleport stuff.
    UpperRoomExit,
    LowerRoomExit,
    UpperRoomGoBack,
    LowerRoomGoBack,
    OpenTheStartDoor,

    // TODO: Add new trigger types as needed.
}

public abstract class RoomInterface : MonoBehaviour
{
    public static Dictionary<Triggers, string> triggerToObjectName = new Dictionary<Triggers, string>
    {
        { Triggers.Gramophone, "Gramophone01/Gramophone" },
        { Triggers.Sponge, "Sponge_for_cleaning" },
        { Triggers.Picture1, "Picture_2" }, // Intentionally swapped, known bug
        { Triggers.Picture2, "Picture_1" },
        { Triggers.Picture3, "Picture_3" },
        { Triggers.Phone, "Phone" },

        // TODO: Add trigger-to-name mappings here as needed.
    };
    public static Dictionary<Triggers, string> triggerToTriggerObjectName = new Dictionary<Triggers, string>
    {
        { Triggers.Phone, "Phone Trigger" },
        { Triggers.Sponge, "Cloth Trigger" },
        { Triggers.Gramophone, "Gramophone Trigger" },
        { Triggers.Window, "Window Trigger" },
        { Triggers.Picture1, "Picture 1 Trigger" },
        { Triggers.Picture2, "Picture 2 Trigger" },
        { Triggers.Picture3, "Picture 3 Trigger" },
        { Triggers.StartDoor, "UpperDoorTrigger" },
        { Triggers.EndDoor, "LowerDoorTrigger" },

        // TODO: Add trigger-to-name mappings here as needed.
    };

    // Which version of the room this currently is.
    public RoomIteration iteration;

    private float highlightEnabledValue = 1;
    private float highlightDisabledValue = 0;

    public string highlightHideShaderPropertyName = "hide";

    // If true, then the next room will be the next iteration, progressing the storyline.
    // If false, then the next room will be the same iteration as this room.
    public bool isCompleted = false;

    // Wrapper around prolog content. For ease of showing/hiding it.
    protected GameObject prologContainer;
    // Wrapper around epilog content. For ease of showing/hiding it.
    protected GameObject epilogContainer;
    // Wrapper around main-room content. For ease of showing/hiding it.
    protected GameObject mainRoomContainer;
    
    virtual public void Arrange(RoomIteration iteration)
    {
        Debug.Log("RoomInterface.Arrange");

        this.iteration = iteration;

        prologContainer = GetReferenceToPrologContainer();
        epilogContainer = GetReferenceToEpilogContainer();
        mainRoomContainer = GetReferenceToMainRoomContainer();

        if (iteration != RoomIteration.Prolog)
        {
            HidePrologContainer();
        }
        if (iteration != RoomIteration.Epilog)
        {
            HideEpilogContainer();
        }
        if (iteration == RoomIteration.Epilog || iteration == RoomIteration.Prolog)
        {
            HideMainRoomContainer();
        }

        SetTriggerIsActive(Triggers.StartDoor, false);
        SetTriggerIsActive(Triggers.EndDoor, false);

        switch (iteration)
        {
            case RoomIteration.Prolog:
                ArrangeForProlog();
                break;
            case RoomIteration.One:
                ArrangeForRoomOne();
                break;
            case RoomIteration.Two:
                ArrangeForRoomTwo();
                break;
            case RoomIteration.Three:
                ArrangeForRoomThree();
                break;
            case RoomIteration.Four:
                ArrangeForRoomFour();
                break;
            case RoomIteration.Five:
                ArrangeForRoomFive();
                break;
            case RoomIteration.Epilog:
                ArrangeForEpilog();
                break;
            case RoomIteration.Unknown:
            default:
                Debug.LogError("Unrecognized RoomIteration: " + iteration);
                break;
        }
    }

    public void OnTriggerTargetChange(Triggers triggerType, bool isTargeted)
    {
        SetObjectHighlight(triggerType, isTargeted);
    }

    protected void SetObjectHighlight(Triggers triggerType, bool isHighlighted)
    {
        GameObject item = GetObjectForTrigger(triggerType);
        if (item == null)
        {
            return;
        }
        float enablementValue = isHighlighted ? highlightEnabledValue : highlightDisabledValue;
        item.GetComponent<Renderer>().material.SetFloat(highlightHideShaderPropertyName, enablementValue);
    }

    protected void SetTriggerIsActive(Triggers triggerType, bool isActivated)
    {
        GameObject item = GetTriggerObjectForTrigger(triggerType);
        if (item != null)
        {
            item.SetActive(isActivated);
        }
    }

    abstract public GameObject GetObjectForTrigger(Triggers triggerType);

    abstract public GameObject GetTriggerObjectForTrigger(Triggers triggerType);

    abstract protected void UnhighlightAllInteractableObjects();

    abstract protected void DeactivateAllInteractableObjects();

    void HidePrologContainer()
    {
        if (prologContainer != null)
        {
            prologContainer.SetActive(false);
        }
    }

    void HideEpilogContainer()
    {
        if (epilogContainer != null)
        {
            epilogContainer.SetActive(false);
        }
    }

    void HideMainRoomContainer()
    {
        if (mainRoomContainer != null)
        {
            mainRoomContainer.SetActive(false);
        }
    }

    protected void QuitToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    abstract public void OnTrigger(Triggers triggerType);

    abstract protected GameObject GetReferenceToPrologContainer();

    abstract protected GameObject GetReferenceToEpilogContainer();

    abstract protected GameObject GetReferenceToMainRoomContainer();

    protected abstract void ArrangeForRoomOne();

    protected abstract void ArrangeForRoomTwo();

    protected abstract void ArrangeForRoomThree();

    protected abstract void ArrangeForRoomFour();

    protected abstract void ArrangeForRoomFive();

    protected abstract void ArrangeForProlog();

    protected abstract void ArrangeForEpilog();
}
