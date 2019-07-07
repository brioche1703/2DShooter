using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagerScript : MonoBehaviour
{

    public void Win()
    {
        PersistentManagerScript.Instance.Win();
    }

    public void Loose()
    {
        PersistentManagerScript.Instance.Loose();
    }

    public void StartGame()
    {
        PersistentManagerScript.Instance.StartGame();
    }
    
    public void QuitGame()
    {
        PersistentManagerScript.Instance.QuitGame();
    }

}
