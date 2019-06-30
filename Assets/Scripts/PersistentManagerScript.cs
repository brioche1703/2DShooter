using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManagerScript : MonoBehaviour
{

    public static PersistentManagerScript Instance { get; private set; }

    // Game stats
    public int enemiesNumber = 4;
    public int enemiesKilled = 0;
    public int coinsNumber;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("PersistentManagerScript Created!");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Menu management
    public void StartGame()
    {
        enemiesNumber = 4;
        Debug.Log("STARTING GAME!!!!!!");
        SceneManager.LoadScene("MainScene");
    } 

    public void QuitGame()
    {
        Debug.Log("Game quit");
        Application.Quit();
    }

    // Game scene management
    public void Win()
    {
        enemiesNumber = 4;
        SceneManager.LoadScene("MainScene");
    }

    public void Loose()
    {
        SceneManager.LoadScene("EndScene");
    }

}
