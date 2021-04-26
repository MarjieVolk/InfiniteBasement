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

    Gramaphone,
    Sponge,
    Picture1,
    Picture2,
    Picture3,
    Phone,

    // TODO: Add new trigger types as needed.
}

public abstract class RoomInterface : MonoBehaviour
{
    // Which version of the room this currently is.
    public RoomIteration iteration;

    // If true, then the next room will be the next iteration, progressing the storyline.
    // If false, then the next room will be the same iteration as this room.
    public bool isCompleted = false;

    // Wrapper around prolog content. For ease of showing/hiding it.
    protected GameObject prologContainer;
    // Wrapper around epilog content. For ease of showing/hiding it.
    protected GameObject epilogContainer;
    // Wrapper around main-room content. For ease of showing/hiding it.
    protected GameObject mainRoomContainer;

    void Start()
    {

    }

    void Update()
    {

    }

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
