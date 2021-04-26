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

    [Tooltip(
        "Define this if you want the Room to be able to identify and react to this trigger.")]
    public Triggers triggerType = Triggers.Unknown;

    /** If true, this trigger has already been triggered before. */
    private bool triggered = false;

    /** Actually run the trigger logic. */
    override protected void OnTrigger() {
        if (triggerOnlyOnce && triggered) {
            Debug.Log("DefaultTrigger.OnTrigger (" + triggerType + "): Ignored because of triggerOnlyOnce.");
            return;
        }
        triggered =  true;

        Debug.Log("DefaultTrigger.OnTrigger (" + triggerType + ")");

        if (playAudioOnTrigger) {
            playAudioOnTrigger.Play();
        }

        RoomArranger.instance.OnTrigger(triggerType);
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