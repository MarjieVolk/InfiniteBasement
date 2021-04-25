using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoom : RoomInterface
{
    private Vector3 placeholderDisplacementBetweeDoors = new Vector3(0.375f, -3.52f, 0);

    override protected GameObject GetReferenceToPrologContent()
    {
        return GameObject.Find("Prolog");
    }

    override protected GameObject GetReferenceToEpilogContent()
    {
        return GameObject.Find("Epilog");
    }

    override protected void ArrangeForRoomOne()
    {
    }

    override protected void ArrangeForRoomTwo()
    {
    }

    override protected void ArrangeForRoomThree()
    {
    }

    override protected void ArrangeForRoomFour()
    {
    }

    override protected void ArrangeForRoomFive()
    {
    }

    override protected void ArrangeForProlog()
    {
        isCompleted = true;
    }

    override protected void ArrangeForEpilog()
    {
        isCompleted = true;
    }
}
