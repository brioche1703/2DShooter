using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCanvaScript : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    public void PauseContinueButton()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
        Time.timeScale = 1;
    }

    public void PauseMainMenuButton()
    {
        Time.timeScale = 1;
        gameObject.GetComponent<Canvas>().enabled = false;
        PersistentManagerScript.Instance.LoadMainMenu();
    }
}
