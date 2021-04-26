using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRoom : RoomInterface
{
    private Vector3 placeholderDisplacementBetweeDoors = new Vector3(0.375f, -3.52f, 0);

    override public GameObject GetObjectForTrigger(Triggers triggerType)
    {
        return null;
    }

    override protected void UnhighlightAllInteractableObjects()
    {
        // TODO
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

    override public void Arrange(RoomIteration iteration)
    {
        SetStartDoorVisible(false);
        SetEndDoorVisible(false);

        base.Arrange(iteration);

        ShowNumberForIteration(iteration);
    }

    void ShowNumberForIteration(RoomIteration iteration)
    {
        string objectName;
        switch (iteration)
        {
            case RoomIteration.One:
                objectName = "One";
                break;
            case RoomIteration.Two:
                objectName = "Two";
                break;
            case RoomIteration.Three:
                objectName = "Three";
                break;
            case RoomIteration.Four:
                objectName = "Four";
                break;
            case RoomIteration.Five:
                objectName = "Five";
                break;
            case RoomIteration.Prolog:
            case RoomIteration.Epilog:
            case RoomIteration.Unknown:
            default:
                return;
        }
        ShowNumber(objectName);
    }

    override protected void ArrangeForRoomOne()
    {
        PlayerController.instance.MarkRoomAsCompleted();
        SetStartDoorVisible(true);
    }

    override protected void ArrangeForRoomTwo()
    {
        PlayerController.instance.MarkRoomAsCompleted();
    }

    override protected void ArrangeForRoomThree()
    {
        PlayerController.instance.MarkRoomAsCompleted();
    }

    override protected void ArrangeForRoomFour()
    {
        PlayerController.instance.MarkRoomAsCompleted();
        SetEndDoorVisible(true);
    }

    override protected void ArrangeForRoomFive()
    {
        // TODO: Unused.
    }

    override protected void ArrangeForProlog()
    {
        // TODO: Unused.
        isCompleted = true;
    }

    override protected void ArrangeForEpilog()
    {
        // TODO: Unused.
        isCompleted = true;
    }

    void ShowNumber(string name)
    {
        string[] numbers = new string[] {
            "One",
            "Two",
            "Three",
            "Four",
            "Five",
        };
        foreach (string otherName in numbers)
        {
            transform.Find("MainRoom/NumbersCanvas/" + otherName).gameObject.SetActive(name == otherName);
        }
    }

    void SetStartDoorVisible(bool visible)
    {
        transform.Find("MainRoom/StartDoor").gameObject.SetActive(visible);
    }

    void SetEndDoorVisible(bool visible)
    {
        transform.Find("MainRoom/EndDoor").gameObject.SetActive(visible);
    }
}
