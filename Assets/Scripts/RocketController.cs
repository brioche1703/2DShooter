using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    int rocketDamage;
    public bool hasBeenTriggered = false;
    CircleCollider2D aoeCollider;

    Rigidbody2D rigidbody2d;
    Character launcherGameObject;
    public GameObject explosion;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); 
        aoeCollider = gameObject.GetComponentInChildren<CircleCollider2D>();
    }

    void Update()
    {
        if (transform.position.magnitude > 100.0f)
            Destroy(gameObject); 
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Character enemy = other.GetComponent<Character>();
        if (enemy != null && !GameObject.ReferenceEquals(enemy, launcherGameObject))
        {
            // Explosion
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity);

            Destroy(expl, 3);
            hasBeenTriggered = true;
        } 
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    public void SetLauncherGameObject(Character launcher)
    {
        launcherGameObject = launcher;
    } 

    public Character GetLauncherGameObject()
    {
        return launcherGameObject;
    } 

    public void SetRocketDamage(int damage)
    {
        rocketDamage = damage;
    } 
}
