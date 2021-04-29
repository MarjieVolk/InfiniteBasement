using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LightType
{
    None,
    Light1,
    Light2,
    Light3,
    Light4,
}

public class LightSwitcher : MonoBehaviour
{
    LightType previousType = LightType.None;
    LightType currentType = LightType.Light1;

    float delaySec = 1;

    Animator[] roomAnimators;

    void Start()
    {
        roomAnimators = GameObject.FindGameObjectsWithTag("RoomAnimator").Select(x => x.GetComponent<Animator>()).ToArray();
    }

    public void IncrementLight()
    {
        SwitchLight(GetNextType(currentType));
    }

    void SwitchLight(LightType type)
    {
        if (type == currentType)
        {
            return;
        }

        previousType = currentType;
        currentType = type;

        string animationName = GetAnimationTriggerNameForLightType(currentType);

        if (currentType == LightType.Light2)
        {
            // Start the curtain-open light immediately.
            TriggerAnimation(animationName);
        }
        else
        {
            // Start after a delay.
            StartCoroutine(DelayedTriggerAnimation(animationName));
        }
    }

    LightType GetNextType(LightType type)
    {
        switch (type)
        {
            case LightType.None:
                return LightType.None;
            case LightType.Light1:
                return LightType.Light2;
            case LightType.Light2:
                return LightType.Light3;
            case LightType.Light3:
                return LightType.Light4;
            case LightType.Light4:
                return LightType.Light4;
            default:
                Debug.LogError("Unrecognized light type: " + type);
                return LightType.None;
        }
    }

    string GetAnimationTriggerNameForLightType(LightType type)
    {
        switch (type)
        {
            case LightType.None:
                return "";
            case LightType.Light1:
                return "";
            case LightType.Light2:
                return "loop1_clear";
            case LightType.Light3:
                return "loop2_clear";
            case LightType.Light4:
                return "loop3_clear";
            default:
                Debug.LogError("Unrecognized light type: " + type);
                return null;
        }
    }

    IEnumerator DelayedTriggerAnimation(string animationName)
    {
        yield return new WaitForSeconds(delaySec);
        TriggerAnimation(animationName);
    }

    public void TriggerAnimation(string animationName)
    {
        Debug.Log("Trigger lights: " + animationName);

        foreach (Animator animator in roomAnimators)
        {
            animator.SetTrigger(animationName);
        }
    }
}
