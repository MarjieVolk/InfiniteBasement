using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSingleton : MonoBehaviour
{

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void StartGame()
    {
        // TODO: Load the real scene whatever that is
        SceneManager.LoadScene("LeviScene");
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)
            || Input.GetKeyDown(KeyCode.Return)
            || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            StartGame();
        }
    }
}
