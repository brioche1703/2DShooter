using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int bulletDamage = 8;

    Rigidbody2D rigidbody2d;
    Character launcherGameObject;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {
        if (transform.position.magnitude > 10.0f)
            Destroy(gameObject); 
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Character enemy = other.GetComponent<Character>();
        if (launcherGameObject != null)
        {
            Character launcher = launcherGameObject.GetComponent<Character>();
            if (enemy != null && !GameObject.ReferenceEquals(enemy, launcher))
            {
                enemy.GetDamaged(bulletDamage, launcherGameObject);
                Destroy(gameObject); 
            } 
        }
        else
        {
            enemy.GetDamaged(bulletDamage, launcherGameObject);
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
}
