using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    int bulletDamage;

    Rigidbody2D rigidbody2d;
    Character launcherGameObject;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>(); 
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
            enemy.GetDamaged(bulletDamage, launcherGameObject);
            Destroy(gameObject); 
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
    public void SetBulletDamage(int damage)
    {
        bulletDamage = damage;
    } 
}
