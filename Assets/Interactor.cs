using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [Tooltip("The tooltip/hint that we show to users when hovering over an object.")]
    public CanvasGroup interactionPrompt;

    // Needed to transform world points to screen space.
    [Tooltip("The player's camera.")]
    public Camera camera;

    [Tooltip("The speed that we should fade in/out the interact tooltip/hint.")]
    public float fadeSpeed = 0.03f;

    private InteractTrigger currentTarget;
    
    // For fading-out after we're no longer highlighting anything.
    private InteractTrigger previousTarget;

    void Start()
    {
        interactionPrompt.alpha = 0;
    }

    void Update()
    {
        renderInteractButton();
    }

    /** Renders a screenspace tooltip that entices the user to press the interact button. */
    private void renderInteractButton()
    {
        RectTransform promptTransform = (RectTransform)(interactionPrompt.gameObject.transform);

        if (currentTarget)
        {
            interactionPrompt.alpha = Mathf.Clamp(interactionPrompt.alpha + fadeSpeed, 0f, 1f);
       
            if (promptTransform != null)
            {
                if (currentTarget.interactPromptTarget != null)
                {
                    promptTransform.position = camera.WorldToScreenPoint(currentTarget.interactPromptTarget.position);
                }
                else
                {
                    // Render to a static spot on the screen?
                    promptTransform.localPosition = new Vector3(-5, -5, 0);
                }
            }
        }
        else
        {
            interactionPrompt.alpha = Mathf.Clamp(interactionPrompt.alpha - fadeSpeed, 0f, 1f);

            if (promptTransform != null && previousTarget != null)
            {
                promptTransform.position = camera.WorldToScreenPoint(previousTarget.interactPromptTarget.position);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision == null)
        {
            return;
        }

        InteractTrigger nextTarget = collision.transform.GetComponent<InteractTrigger>();

        if (nextTarget == null)
        {
            return;
        }

        if (nextTarget != currentTarget)
        {
            if (currentTarget != null)
            {
                currentTarget.SetIsTargeted(false);
            }

            previousTarget = currentTarget;
            currentTarget = nextTarget;

            if (nextTarget != null)
            {
                nextTarget.SetIsTargeted(true);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        InteractTrigger pastTarget = collision.transform.GetComponent<InteractTrigger>();

        if (currentTarget == pastTarget && currentTarget != null)
        {
            currentTarget.SetIsTargeted(false);
            previousTarget = currentTarget;
            currentTarget = null;
        }
    }
}
