using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndSceneManagerScript : MonoBehaviour
{
    public void QuitGame()
    {
        PersistentManagerScript.Instance.QuitGame();
    } 

    public void LoadMainMenu()
    {
        PersistentManagerScript.Instance.LoadMainMenu();
    }
}
