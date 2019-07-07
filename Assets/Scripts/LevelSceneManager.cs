using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSceneManager : MonoBehaviour
{

    protected bool levelCleared = false;
    protected SimpleTimer winDelayTimer;
    protected float winDelayTimeDuration = 3.0f;
    protected SimpleTimer levelTimeTimer;
    protected float levelTimeDuration = 60.0f;

    public int coinsEarned;
    public int scoreEarned;

    // UI
    public Image[] hearts;
    public Text coinsText; 
    public Text scoreText; 
    public Text enemiesKilledText; 
    public Text timeText; 
    public Text rocketText; 


    void Awake()
    {
        coinsEarned = 0;
        scoreEarned = 0;
        
        // UI 
        UpdateHeartsUI();
        UpdateCoinsTextUI();
        UpdateEnemiesKilledTextUI();
        UpdateRocketTextUI();
        
        // Timers 
        winDelayTimer = gameObject.AddComponent<SimpleTimer>(); 
        levelTimeTimer = gameObject.AddComponent<SimpleTimer>(); 
        levelTimeTimer.StartTimer(levelTimeDuration);

    }

    void Update()
    {
        // UI
        UpdateCoinsTextUI();
        UpdateEnemiesKilledTextUI();
        UpdateTimeTextUI();
        UpdateScoreTextUI();
        UpdateRocketTextUI();

        if (levelTimeTimer.isFinished())
        {
            PersistentManagerScript.Instance.Loose();
        }
        if (isLevelCleared())
        {
            if (winDelayTimer.isFinished())
            {
                PersistentManagerScript.Instance.coins += coinsEarned;
                PersistentManagerScript.Instance.score += scoreEarned;
                PersistentManagerScript.Instance.Win();            
            }
        }
    }

    void UpdateHeartsUI()
    {
        int nbLives = PersistentManagerScript.Instance.p_lives;
        for (int i = 0; i < PersistentManagerScript.Instance.MAX_LIVES; i++)
        {
            if (i < nbLives)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void UpdateCoinsTextUI()
    {
        coinsText.text = (PersistentManagerScript.Instance.coins + coinsEarned).ToString(); 
    }
    
    void UpdateScoreTextUI()
    {
        scoreText.text = "Score: " + (PersistentManagerScript.Instance.score + scoreEarned).ToString(); 
    }

    void UpdateEnemiesKilledTextUI()
    {
        enemiesKilledText.text = (PersistentManagerScript.Instance.enemiesKilled + 
                PersistentManagerScript.Instance.enemiesInLevelKilled).ToString(); 
    }
    
    void UpdateTimeTextUI()
    {
        timeText.text = ((int)levelTimeTimer.GetCurrentTime()).ToString();
    }

    void UpdateRocketTextUI()
    {
        rocketText.text = PersistentManagerScript.Instance.p_rocketAmmo.ToString();
    }

    bool isLevelCleared()
    {
    // Check if level is won
        if (PersistentManagerScript.Instance.enemiesDead
                == PersistentManagerScript.Instance.enemiesNumber)
        {
            // Start winDelayTimer
            if (!levelCleared) {
                winDelayTimer.StartTimer(winDelayTimeDuration);
                levelCleared = true;
            }
            return true;
        }
        return false;
    } 
}
