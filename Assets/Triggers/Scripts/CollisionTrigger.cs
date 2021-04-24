using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : DefaultTrigger
{
    [Tooltip(
        "Required. The object that will trigger me when it enters the trigger zone." +
        " To set this, simply drag the triggering game object (e.g. the player) here." +
        " I recommend saving a pre-fab with this value already set, so you don't have" +
        " to keep setting it.")]
    public Collider triggerTarget;

    void OnTriggerEnter(Collider collision) {
        if (collision != triggerTarget) {
            return;
        }

        OnTrigger();
    }

    void Start() {
        
    }

    void Update() {
        
    }
}
