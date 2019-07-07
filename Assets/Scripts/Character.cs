using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    // Default caracteristics 
    protected int startingHealth = 100;
    
    protected int startingShield = 40;
    protected int shieldRecoverSpeed = 1;
    protected float shieldRecoverRate = 10.0f;

    public int bulletDamage = 30;
    public int bulletSpeed = 100;
    public int rocketDamage = 50;
    public int rocketSpeed = 150;
    protected int collisionDamage = 10;
    float collisionPvPMagnitude = 2.0f;
    float collisionPvEMagnitude = 0.1f;

    public float speed = 3.0f; 
    // Current
    protected int health;
    protected int shield;
    protected bool isLastEnemyDamageFromPlayer = false;
    
    // Timers
    protected SimpleTimer bouncingTimer;
    protected SimpleTimer shieldRecoverTimer;
    protected SimpleTimer shieldRecoverRateTimer;
    
    // Timer Durations
    protected float bouncingDuration = 2.0f; 
    protected float shieldRecoverTimeDuration = 10.0f;
    protected float shieldRecoverRateDuration = 0.5f;


    // Other
    public GameObject bulletPrefab;
    public GameObject rocketPrefab;
    public HealthBar healthBar;
    public ShieldBar shieldBar;
    protected Rigidbody2D rigidbody2d;

    protected Vector2 lookDirection = new Vector2(1,0);
    protected Quaternion lookRotation = Quaternion.identity;

    // UI
    protected Vector3 healthBarOffset;
    protected Vector3 shieldBarOffset;

    protected virtual void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        // Timers
        bouncingTimer = gameObject.AddComponent<SimpleTimer>(); 
        shieldRecoverTimer = gameObject.AddComponent<SimpleTimer>(); 
        shieldRecoverRateTimer = gameObject.AddComponent<SimpleTimer>(); 
    }

    protected virtual void Start()
    {
        health = startingHealth;
        shield = startingShield;
        healthBarOffset = -(transform.position - healthBar.transform.position);
        shieldBarOffset = -(transform.position - shieldBar.transform.position);
   
    }

    protected virtual void Update()
    {
        if (isDead())
        {
            GetDestroy();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        bouncingTimer.StartTimer(bouncingDuration);

        Character enemy = other.gameObject.GetComponent<Character>();

        Vector2 direction;
        float collisionMagnitude;
        if (enemy != null)
        {
            isLastEnemyDamageFromPlayer = enemy.isPlayer();
            direction = new Vector2(enemy.transform.position.x,
                enemy.transform.position.y) - rigidbody2d.position;
            collisionMagnitude = collisionPvPMagnitude;
        }
        else
        {
            direction = rigidbody2d.position - Vector2.zero;
            collisionMagnitude = collisionPvEMagnitude;
        }
        rigidbody2d.velocity = Vector2.zero;
        rigidbody2d.angularVelocity = 0.0f;
        rigidbody2d.AddForce(-direction * collisionMagnitude, ForceMode2D.Impulse);
        GetDamaged(collisionDamage);
    }

    protected bool isBouncing()
    {
        return !bouncingTimer.isFinished();
    }

    protected bool isDead()
    {
        return health <= 0;
    }

    public virtual void GetDestroy()
    {
        Destroy(gameObject);
    }

    public void GetDamaged(int damage)
    {
        shield -= damage;
        shieldRecoverTimer.StartTimer(shieldRecoverTimeDuration);
        
        if (shield <= 0)
        {
            health -= damage - shield;
            shield = 0;
        }
    }
    public void GetDamaged(int damage, Character enemy)
    {
        isLastEnemyDamageFromPlayer = enemy.isPlayer();
        Debug.Log("DMG from Player? " + isLastEnemyDamageFromPlayer);
        shield -= damage;
        shieldRecoverTimer.StartTimer(shieldRecoverTimeDuration);
        
        if (shield <= 0)
        {
            health -= damage - shield;
            shield = 0;
        }
    }

    public void LaunchBullet()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, rigidbody2d.position + lookDirection * new Vector2(0.5f, 0.5f), Quaternion.identity);
        BulletController bullet = bulletObject.GetComponent<BulletController>();
        bullet.SetLauncherGameObject(this);
        bullet.SetBulletDamage(bulletDamage);
        bullet.Launch(lookDirection, bulletSpeed);
    }

    public void LaunchRocket()
    {
        GameObject rocketObject = Instantiate(rocketPrefab, rigidbody2d.position + lookDirection * new Vector2(0.5f, 0.5f), lookRotation);
        RocketController rocket = rocketObject.GetComponent<RocketController>();
        rocket.SetLauncherGameObject(this);
        rocket.SetRocketDamage(rocketDamage);
        rocket.Launch(lookDirection, rocketSpeed);
    }

    public virtual void RecoverShield()
    {
        if (shieldRecoverTimer.isFinished())
        {
            // Small timer to avoid shield instant recover
            if (shieldRecoverRateTimer.isFinished())
            {
                shieldRecoverRateTimer.StartTimer(shieldRecoverRateDuration);
            }
            else
            {
                shield = Mathf.Clamp(shield + 1, 0, startingShield);
            }
        }    
    }

    public virtual bool isPlayer() 
    { 
        return false;
    }
}
