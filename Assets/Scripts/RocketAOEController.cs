using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAOEController : MonoBehaviour
{

    public int aoeDamage = 50; 

    void OnTriggerStay2D(Collider2D other) 
    {
        RocketController parentController = GetComponentInParent<RocketController>(); 
        Character enemy = other.GetComponent<Character>();


        if (enemy != null && !GameObject.ReferenceEquals(enemy, parentController.GetLauncherGameObject()) && parentController.hasBeenTriggered) 
        {
            enemy.GetDamaged(aoeDamage, parentController.GetLauncherGameObject());
            Destroy(gameObject); 
            Destroy(parentController.gameObject); 
        }
    }    
}
