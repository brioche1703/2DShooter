using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    // Character caracteristics 
    // Starting
    protected int startingHealth = 100;

    protected int startingShield = 40;
    protected int shieldRecoverSpeed = 1;
    protected float shieldRecoverRate = 2.0f;

    protected int startingRocketsNumber = 3;

    protected int startingLifeNumber = 3;

    protected int collisionDamage = 10;

    public float speed = 3.0f; 
    // Current
    protected int lives;
    protected int health;
    protected int shield;
    protected int leftRockets;
    
    // Timers & conditions
    protected float shieldRecoverTimer = 0.0f;
    protected float shieldRecoverRateTimer = 0.0f;
    protected bool isShieldRecovering = true;

    // Other
    public GameObject bulletPrefab;
    public HealthBar healthBar;
    public ShieldBar shieldBar;
    protected Rigidbody2D rigidbody2d;

    protected Vector2 lookDirection = new Vector2(1,0);

    // UI
    protected Vector3 healthBarOffset;
    protected Vector3 shieldBarOffset;

    // Bouncing collision
    protected float bouncingTimer = 0.0f; 
    protected float bouncingTime = 2.0f; 

    protected virtual void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        health = startingHealth;
        shield = startingShield;
        healthBarOffset = -(transform.position - healthBar.transform.position);
        shieldBarOffset = -(transform.position - shieldBar.transform.position);
    }

    protected virtual void Update()
    {
        UpdateBars();
        RecoverShield();

        bouncingTimer -= Time.deltaTime;
        if (bouncingTimer <= 0.0f)
        {
            rigidbody2d.velocity = Vector3.zero;
            rigidbody2d.angularVelocity = 0.0f;
        }
        
        isLevelCleared();

    }

    void isLevelCleared()
    {
        // Check if level is won
        if (PersistentManagerScript.Instance.enemiesNumber <= 0)
        {
            PersistentManagerScript.Instance.Win();
        }

    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        bouncingTimer = bouncingTime;
        Character enemy = other.gameObject.GetComponent<Character>();
        if (enemy != null)
        {
            GetDamaged(collisionDamage, enemy);
        }
            
        Vector2 direction = new Vector2(enemy.transform.position.x,
                enemy.transform.position.y) - rigidbody2d.position;
        rigidbody2d.AddForce(-direction * 2.0f, ForceMode2D.Impulse);
    }

    protected bool isDead()
    {
        return health <= 0;
    }
    
    public virtual void GetDestroy()
    {
        Destroy(gameObject);
    }
    
    public void GetDamaged(int damage, Character enemy)
    {
        shield -= damage;
        shieldRecoverTimer = 5.0f;
        if (shield <= 0)
        {
            health -= damage - shield;
            shield = 0;
        }
        if (isDead())
        {
            PlayerController player = enemy.GetComponent<PlayerController>();
            if (player != null)
            {
                this.GetDestroy();
            }
            else 
            {
                this.GetDestroy();
            }
        }
    }

    void RecoverShield()
    {
        // Recover Shield
        shieldRecoverTimer -= Time.deltaTime;

        if (shieldRecoverTimer <= 0)
        {
            if (!isShieldRecovering)
            {
                isShieldRecovering = true;
                shieldRecoverRateTimer = shieldRecoverRate;
            }
            shieldRecoverRateTimer -= Time.deltaTime;
            if (shieldRecoverRateTimer <= 0)
                shield = Mathf.Clamp(shield + 1, 0, startingShield);
        }    
        else
        {
            isShieldRecovering = false;
        }
    }

    void UpdateBars()
    {
        healthBar.SetSize((float)health/(float)startingHealth);
        shieldBar.SetSize((float)shield/(float)startingShield);
    }
}
