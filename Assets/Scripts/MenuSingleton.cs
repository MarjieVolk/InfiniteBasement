using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSingleton : MonoBehaviour
{
    [Tooltip("The group that we'll fade to black on menu outro")]
    public CanvasGroup fadeGroup;

    [Tooltip("Amount (fraction, 1 = 100%) we should fade out per second.")]
    public float fadeSpeed = 1;

    private bool isFadingToBlack = false;

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void StartGame()
    {
        // TODO: Load the real scene whatever that is
        SceneManager.LoadScene("AudreyScene");
    }
    
    public void StartFadeOut()
    {
        isFadingToBlack = true;
    }

    public void FadeOut()
    {
        if (fadeGroup.alpha > 0.0f) {
            fadeGroup.alpha -= fadeSpeed * Time.deltaTime;
        } else {
            StartGame();
        }
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartFadeOut();
        }

        if (isFadingToBlack)
        {
            FadeOut();
        }
    }
}
