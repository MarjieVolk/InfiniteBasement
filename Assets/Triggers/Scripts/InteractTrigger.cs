using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An event that can occur due to a player interaction.
 *
 * This trigger should be used with an {@link Interactor}.
 */
public class InteractTrigger : DefaultTrigger
{
    [Tooltip("The location where we should display the interaction tooltip/hint.")]
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
            //Debug.Log("InteractTrigger.SetIsTargeted (" + triggerType + "): " + isTargeted);
            RoomArranger.instance.OnTriggerTargetChange(triggerType, isTargeted);
        }
    }
}
