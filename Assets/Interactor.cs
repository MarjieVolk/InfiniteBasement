using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    public CanvasGroup interactionPrompt;
    public Camera camera;

    private InteractTrigger currentTarget;
    private InteractTrigger previousTarget;

    // Start is called before the first frame update
    void Start()
    {
        interactionPrompt.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform promptTransform = (RectTransform)(interactionPrompt.gameObject.transform);

        if (currentTarget)
        {
            interactionPrompt.alpha = Mathf.Clamp(interactionPrompt.alpha + 0.01f, 0f, 1f);
       
            if (promptTransform != null)
            {
                if (currentTarget.interactPromptTarget != null)
                {
                    promptTransform.position = camera.WorldToScreenPoint(currentTarget.interactPromptTarget.position);
                }
                else
                {
                    promptTransform.localPosition = new Vector3(-5, -5, 0);
                }
            }
        }
        else
        {
            interactionPrompt.alpha = Mathf.Clamp(interactionPrompt.alpha - 0.01f, 0f, 1f);

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
