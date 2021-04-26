using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : DefaultTrigger
{
    public Transform interactPromptTarget;

    private bool isTargeted = false;

    void Start() {
        
    }

    void Update()
    {
        // Any button!!!
        if (isTargeted && 
            (Input.GetKeyDown(KeyCode.E)
            || Input.GetMouseButtonDown(0)
            || Input.GetMouseButtonDown(1)
            || Input.GetKeyDown(KeyCode.KeypadEnter)
            || Input.GetKeyDown(KeyCode.Return)))
        {
            OnTrigger();
        }
    }

    public void SetIsTargeted(bool isTargeted)
    {
        bool changed = isTargeted != this.isTargeted;
        this.isTargeted = isTargeted;
        if (changed)
        {
            Debug.Log("InteractTrigger.SetIsTargeted (" + triggerType + "): " + isTargeted);
            RoomArranger.instance.OnTriggerTargetChange(triggerType, isTargeted);
        }
    }
}
