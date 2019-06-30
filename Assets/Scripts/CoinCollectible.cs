using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour
{

    float lifeTimer = 3.0f;

    void Update()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0.0f)
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
