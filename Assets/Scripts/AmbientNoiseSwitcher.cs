using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundNoise
{
    Basement,
    Outdoors,
}

public class AmbientNoiseSwitcher : MonoBehaviour
{
    public static AmbientNoiseSwitcher instance { get; private set; }

    public float crossFadeDurationSec = 2f;
    public float volume = 0.6f;

    public AudioSource basementSource;
    public AudioSource outdoorSource;
    
    BackgroundNoise currentType = BackgroundNoise.Basement;

    AudioSource previousSource;
    AudioSource currentSource;

    protected void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            ForcePlayBackgroundNoise(BackgroundNoise.Basement);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PlayBackgroundNoise(BackgroundNoise type)
    {
        if (type == currentType)
        {
            return;
        }

        ForcePlayBackgroundNoise(type);
    }

    private void ForcePlayBackgroundNoise(BackgroundNoise type)
    {
        currentType = type;

        previousSource = currentSource;
        currentSource = GetAudioSource(currentType);
        
        if (previousSource != null)
        {
            StartCoroutine(FadeAudioSource.StartFade(previousSource, crossFadeDurationSec, 0));
        }

        if (currentSource != null)
        {
            StartCoroutine(FadeAudioSource.StartFade(currentSource, crossFadeDurationSec, volume));
        }
    }

    private AudioSource GetAudioSource(BackgroundNoise type)
    {
        switch (type)
        {
            case BackgroundNoise.Basement:
                return basementSource;
            case BackgroundNoise.Outdoors:
                return outdoorSource;
            default:
                Debug.LogError("Unrecognized background noise type: " + type);
                return null;
        }
    }
}
