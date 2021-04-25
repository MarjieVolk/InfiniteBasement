using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : DefaultTrigger
{
    public void OnInteract() {
        OnTrigger();
    }

    void Start() {
        
    }

    void Update() {
        // Temp logic to trigger, until we have a char I can add interact to.
        if (Input.GetKeyDown("space"))
        {
            OnTrigger();
        }
    }
}
