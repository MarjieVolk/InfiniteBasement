using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomIteration
{
    // This represents whatever is shown before entering the first door, at the start of the game.
    Prolog,

    One,
    Two,
    Three,
    Four,
    Five,
    
    // This represents whatever is shown after leaning the last door, at the end of the game.
    Epilog,
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

    public void Arrange(RoomIteration iteration)
    {
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
            default:
                Debug.LogError("Unrecognized RoomIteration: " + iteration);
                break;
        }
    }

    void HidePrologContainer()
    {
        prologContainer.SetActive(false);
    }

    void HideEpilogContainer()
    {
        epilogContainer.SetActive(false);
    }

    void HideMainRoomContainer()
    {
        mainRoomContainer.SetActive(false);
    }

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
