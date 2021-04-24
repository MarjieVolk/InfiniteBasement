using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefaultTrigger : Trigger
{

    [Tooltip("If true, this trigger will only ever trigger once.")]
    public bool triggerOnlyOnce = false;

    [Tooltip(
        "If present, this trigger will play this audio clip when triggered. To use this, enable" +
        " the audio source on this object, and add an audio clip to it. Then drag the audio source" +
        " component here.")]
    public AudioSource playAudioOnTrigger = null;

    /** If true, this trigger has already been triggered before. */
    private bool triggered = false;

    /** Actually run the trigger logic. */
    override protected void OnTrigger() {
        if (triggerOnlyOnce && triggered) {
            return;
        }
        triggered =  true;

        if (playAudioOnTrigger) {
            playAudioOnTrigger.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    
}