using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSceneManager : MonoBehaviour
{

    public void StartGame()
    {
        PersistentManagerScript.Instance.StartGame();
    }

    public void StartAutopilot()
    {
        PersistentManagerScript.Instance.StartAutopilot();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
