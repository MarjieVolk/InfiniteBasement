using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSingleton : MonoBehaviour
{
    [Tooltip("The group that we'll fade to black on menu outro")]
    public CanvasGroup fadeGroup;

    public CanvasGroup foregroundGroup;

    [Tooltip("Amount (fraction, 1 = 100%) we should fade out per second.")]
    public float fadeSpeed = 1;

    private bool isFadingToBlack = false;

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    
    public void StartFadeOut()
    {
        isFadingToBlack = true;
    }

    protected void Start()
    {
        fadeGroup.alpha = 1;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
            if (fadeGroup.alpha > 0.0f)
            {
                fadeGroup.alpha -= fadeSpeed * Time.deltaTime;
            }
            else
            {
                SceneManager.LoadScene("AudreyScene");
            }
        }

        float prevAlpha = foregroundGroup.alpha;
        foregroundGroup.alpha = 1f - (1f / (1f + Mathf.Pow(2.718f, -3f * (Time.timeSinceLevelLoad - 2.5f))));

        if (foregroundGroup.alpha < 0.5f && prevAlpha >= 0.5f)
        {
            foregroundGroup.blocksRaycasts = false;
        }
    }
}
