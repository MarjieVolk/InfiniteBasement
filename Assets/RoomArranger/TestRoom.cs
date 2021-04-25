using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestRoom : RoomInterface
{
    private Vector3 placeholderDisplacementBetweeDoors = new Vector3(0.375f, -3.52f, 0);

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
        PlayerController.instance.MarkRoomAsCompleted();
        ShowNumber("One");
    }

    override protected void ArrangeForRoomTwo()
    {
        ShowNumber("Two");
    }

    override protected void ArrangeForRoomThree()
    {
        ShowNumber("Three");
    }

    override protected void ArrangeForRoomFour()
    {
        ShowNumber("Four");
    }

    override protected void ArrangeForRoomFive()
    {
        ShowNumber("Five");
    }

    override protected void ArrangeForProlog()
    {
        isCompleted = true;
    }

    override protected void ArrangeForEpilog()
    {
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
}
