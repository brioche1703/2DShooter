using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManagerScript : MonoBehaviour
{

    public static PersistentManagerScript Instance { get; private set; }

    // Game specificities
    public int MAX_LIVES = 3;
    public int MAX_HEALTH = 100; 
    public int MAX_SHIELD = 100; 
    public float MIN_SHIELD_RECOVER_RATE = 2.0f;
    public int MAX_ROCKET_AMMO_CAPACITY = 10;

    // Character starting stats
    public int p_lives;
    public int p_startingHealth; 
    public int p_startingShieldCapacity; 
    public float p_shieldRecoverTimeDuration;
    public float p_shieldRecoverRateDuration;
    public int p_bulletDamage;
    public int p_bulletSpeed;
    public int p_rocketDamage;
    public int p_rocketAmmoCurrentCapacityMax;
    public int p_rocketAmmo;

    // Game stats
    public int enemiesNumber;
    public int enemiesKilled;
    public int enemiesInLevelKilled;
    public int enemiesDead;
    public int score;
    public int coins;

    // Testing mode
    public bool autopilot = false;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialisation of game variables
    void InitGame()
    {
        autopilot = false;
        p_lives = 3;
        p_startingHealth = 100;
        p_startingShieldCapacity = 40;
        p_shieldRecoverTimeDuration = 10.0f;
        p_shieldRecoverRateDuration = 0.5f;
        p_bulletDamage = 20;
        p_bulletSpeed = 200;
        p_rocketAmmo = 3;
        p_rocketAmmoCurrentCapacityMax = 3;

        enemiesNumber = 2;
        enemiesKilled = 0;
        enemiesInLevelKilled = 0;
        enemiesDead = 0;
        score = 0;
        coins = 0;
    }

    void InitLevelScene()
    {
        enemiesDead = 0;
        enemiesInLevelKilled = 0;
    }

    // Menu management
    public void StartGame()
    {
        InitGame();
        InitLevelScene();
        LoadLevelScene();
    } 

    public void StartAutopilot()
    {
       autopilot = true;
       p_bulletDamage = 100;
       LoadLevelScene();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Game scene management
    public void Win()
    {
        if (autopilot)
        {
            enemiesNumber += 10;
            enemiesKilled += enemiesInLevelKilled;
            LoadLevelScene();
        }
        else
        {
            enemiesNumber++;
            score += 100;
            enemiesKilled += enemiesInLevelKilled;
            SceneManager.LoadScene("ShopScene");
        }
    }

    public void Loose()
    {
        p_lives--;
        if (p_lives > 0)
        {

            LoadLevelScene();
        }
        else
        {
            score += coins * 5;
            SceneManager.LoadScene("EndScene");
        }
    }

    public void LoadLevelScene()
    {
        InitLevelScene();
        SceneManager.LoadScene("LevelScene");
    }

    public void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }

    public void LoadMainMenu()
    {
        InitGame();
        SceneManager.LoadScene("MainMenu");
    }
}
