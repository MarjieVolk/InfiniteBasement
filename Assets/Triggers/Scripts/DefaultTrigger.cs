using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An default trigger that provides some simple functionality for sounds, etc.
 *
 * For more complex logic, you may need to define a new trigger instead of using this class.
 */
public abstract class DefaultTrigger : Trigger
{

    [Tooltip("If true, this trigger will only ever trigger once.")]
    public bool triggerOnlyOnce = false;

    [Tooltip(
        "Required: The component that will play the audio.")]
    public AudioSource audioPlayer = null;

    [Tooltip(
        "If present, this trigger will play this audio clip when triggered. To use this, enable" +
        " the audio source on this object.")]
    public AudioClip playAudioOnTrigger = null;

    [Tooltip(
        "If present, this trigger will play these audio clips when triggered. On each use, it will"
        + " move to the next item in the list.")]
    public AudioClip[] playAudioOnDisabled = {};

    [Tooltip("Whether the player can interact with this.")]
    public bool isEnabled = true;

    [Tooltip(
        "Define this if you want the Room to be able to identify and react to this trigger.")]
    public Triggers triggerType = Triggers.Unknown;

    /** If true, this trigger has already been triggered before. */
    private bool triggered = false;

    /** The number of times the player interacted with this trigger when it was disabled. For audio. */
    private int numTimesDisabled = 0;

    /** Actually run the trigger logic. */
    override protected void OnTrigger() {
        if (triggerOnlyOnce && triggered) {
            Debug.Log("DefaultTrigger.OnTrigger (" + triggerType + "): Ignored because of triggerOnlyOnce.");
            return;
        }

        Debug.Log("DefaultTrigger.OnTrigger (" + triggerType + ")");

        if (isEnabled && playAudioOnTrigger) {
            audioPlayer.clip = playAudioOnTrigger;
            audioPlayer.Play();
            triggered =  true;
            RoomArranger.instance.OnTrigger(triggerType, isEnabled);
        } else if (!isEnabled && playAudioOnDisabled.Length > 0) {
            audioPlayer.clip = playAudioOnDisabled[numTimesDisabled++ % playAudioOnDisabled.Length];
            audioPlayer.Play();
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