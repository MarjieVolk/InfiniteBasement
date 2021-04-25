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
        // TODO: Sorry Steven! Levi broke this. I was having trouble getting the prefab reference to work, I guess? Please revert if you want!
        //if (collision != triggerTarget) {
        if (collision.tag != "Player") {
            return;
        }

        OnTrigger();
    }

    void Start() {
        
    }

    void Update() {
        
    }
}
