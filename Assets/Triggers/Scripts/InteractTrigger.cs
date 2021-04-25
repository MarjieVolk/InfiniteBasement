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
        if (isTargeted && Input.GetKeyDown("e"))
        {
            OnTrigger();
        }

        // TODO show/hide interact prompt
    }

    public void SetIsTargeted(bool isTargeted)
    {
        this.isTargeted = isTargeted;
    }
}
