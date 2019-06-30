using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    
    // Menu Scene
    public void StartGame()
    {
        PersistentManagerScript.Instance.StartGame();
    }

    public void QuitGame()
    {
        PersistentManagerScript.Instance.QuitGame();
    }

    // Game scene
    public void Win()
    {
        PersistentManagerScript.Instance.enemiesNumber++;
        PersistentManagerScript.Instance.Win();
    }

    public void Loose()
    {
        PersistentManagerScript.Instance.Loose();
    }
}
