using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour
{

    SimpleTimer lifeTimer;
    float lifeDuration = 3.0f;

    void Awake()
    {
        lifeTimer = gameObject.AddComponent<SimpleTimer>();
        lifeTimer.StartTimer(lifeDuration);
    }

    void Update()
    {
        if (lifeTimer.isFinished())
        {
            Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

       if (player != null)
       {
           player.GetCoin();
           Destroy(gameObject);
       } 
    }
}
