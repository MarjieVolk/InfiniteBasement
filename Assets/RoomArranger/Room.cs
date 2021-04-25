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

public class Room : MonoBehaviour
{
    // Which version of the room this currently is.
    public RoomIteration iteration;

    // If true, then the next room will be the next iteration, progressing the storyline.
    // If false, then the next room will be the same iteration as this room.
    public bool isCompleted = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Arrange(RoomIteration iteration)
    {
        this.iteration = iteration;

        // TODO: Remove this once we're done using the placeholder content.
        PlaceholderArrange();
        return;


        HidePrologContent();
        HideEpilogContent();

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

    void ArrangeForRoomOne()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    void ArrangeForRoomTwo()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    void ArrangeForRoomThree()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    void ArrangeForRoomFour()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    void ArrangeForRoomFive()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    void ArrangeForProlog()
    {
        isCompleted = true;
        // TODO: Show/hide/move/adjust whatever items/state is needed for the prolog.
    }

    void ArrangeForEpilog()
    {
        isCompleted = true;
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    void HidePrologContent()
    {
        // TODO:
    }

    void HideEpilogContent()
    {
        // TODO:
    }


    // --------------------------------------------------------------------- //
    // TODO: Remove all the stuff below this line, which is temporarily being used for placeholder content.

    private Vector3 placeholderDisplacementBetweeDoors = new Vector3(0.375f, -3.52f, 0);

    private void PlaceholderArrange()
    {
        PlaceholderHidePrologContent();
        PlaceholderHideEpilogContent();

        switch (iteration)
        {
            case RoomIteration.Prolog:
                PlaceholderArrangeForProlog();
                break;
            case RoomIteration.One:
                PlaceholderArrangeForRoomOne();
                break;
            case RoomIteration.Two:
                PlaceholderArrangeForRoomTwo();
                break;
            case RoomIteration.Three:
                PlaceholderArrangeForRoomThree();
                break;
            case RoomIteration.Four:
                PlaceholderArrangeForRoomFour();
                break;
            case RoomIteration.Five:
                PlaceholderArrangeForRoomFive();
                break;
            case RoomIteration.Epilog:
                PlaceholderArrangeForEpilog();
                break;
            default:
                Debug.LogError("Unrecognized RoomIteration: " + iteration);
                break;
        }
    }

    void PlaceholderArrangeForRoomOne()
    {
    }

    void PlaceholderArrangeForRoomTwo()
    {
    }

    void PlaceholderArrangeForRoomThree()
    {
    }

    void PlaceholderArrangeForRoomFour()
    {
    }

    void PlaceholderArrangeForRoomFive()
    {
    }

    void PlaceholderArrangeForProlog()
    {
        isCompleted = true;
        GameObject.Find("Prolog").SetActive(true);
    }

    void PlaceholderArrangeForEpilog()
    {
        isCompleted = true;
        GameObject.Find("Epilog").SetActive(true);
    }

    void PlaceholderHidePrologContent()
    {
        GameObject.Find("Prolog").SetActive(false);
    }

    void PlaceholderHideEpilogContent()
    {
        GameObject.Find("Epilog").SetActive(false);
    }
}
