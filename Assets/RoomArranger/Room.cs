using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : RoomInterface
{
    void Start()
    {

    }

    void Update()
    {

    }

    override public void OnTrigger(Triggers triggerType)
    {
        switch (triggerType)
        {
            // TODO: React to individual triggers.
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

    override protected void ArrangeForRoomOne()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomTwo()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomThree()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomFour()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForRoomFive()
    {
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }

    override protected void ArrangeForProlog()
    {
        isCompleted = true;
        // TODO: Show/hide/move/adjust whatever items/state is needed for the prolog.
    }

    override protected void ArrangeForEpilog()
    {
        isCompleted = true;
        // TODO: Show/hide/move/adjust whatever items/state is needed for the current room.
    }
}
