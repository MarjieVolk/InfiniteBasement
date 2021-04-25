using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterRoomTrigger : CollisionTrigger
{
    void Start()
    {
        triggerOnlyOnce = true;
    }

    override protected void OnTrigger()
    {
        base.OnTrigger();
        PlayerController.instance.OnRoomEntered();
    }
}
