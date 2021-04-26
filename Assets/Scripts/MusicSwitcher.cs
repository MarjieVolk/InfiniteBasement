using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music
{
    None,
    Piano1,
    Piano2,
    Piano3,
    Piano4,
}

public class MusicSwitcher : MonoBehaviour
{
    AudioSource pianoSource1;
    AudioSource pianoSource2;
    AudioSource pianoSource3;
    AudioSource pianoSource4;

    Music previousType = Music.None;
    Music currentType = Music.None;

    AudioSource previousSource;
    AudioSource currentSource;

    public float crossFadeDurationSec = 1;

    void Start()
    {
        pianoSource1 = transform.Find("PianoSource1").gameObject.GetComponent<AudioSource>();
        pianoSource2 = transform.Find("PianoSource2").gameObject.GetComponent<AudioSource>();
        pianoSource3 = transform.Find("PianoSource3").gameObject.GetComponent<AudioSource>();
        pianoSource4 = transform.Find("PianoSource4").gameObject.GetComponent<AudioSource>();
    }

    public void IncrementMusic()
    {
        PlayMusic(GetNextType(currentType));
    }

    public void PlayMusic(Music type)
    {
        if (type == currentType)
        {
            return;
        }

        previousType = currentType;
        currentType = type;

        previousSource = currentSource;
        currentSource = GetAudioSourceForMusicType(currentType);

        if (previousType != Music.None)
        {
            // Cross fade.}
            StartCoroutine(FadeAudioSource.StartFade(previousSource, crossFadeDurationSec, 0));
            StartCoroutine(FadeAudioSource.StartFade(currentSource, crossFadeDurationSec, 1));
        }
        else
        {
            // Start immediately.
            StartCoroutine(FadeAudioSource.StartFade(currentSource, 0.01f, 1));
        }
    }

    Music GetNextType(Music type)
    {
        switch (type)
        {
            case Music.None:
                return Music.Piano1;
            case Music.Piano1:
                return Music.Piano2;
            case Music.Piano2:
                return Music.Piano3;
            case Music.Piano3:
                return Music.Piano4;
            case Music.Piano4:
                return Music.Piano4;
            default:
                Debug.LogError("Unrecognized music type: " + type);
                return Music.None;
        }
    }

    AudioSource GetAudioSourceForMusicType(Music type)
    {
        switch (type)
        {
            case Music.None:
                return null;
            case Music.Piano1:
                return pianoSource1;
            case Music.Piano2:
                return pianoSource2;
            case Music.Piano3:
                return pianoSource3;
            case Music.Piano4:
                return pianoSource4;
            default:
                Debug.LogError("Unrecognized music type: " + type);
                return null;
        }
    }
}

public static class FadeAudioSource
{
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        if (audioSource == null)
        {
            yield break;
        }

        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
