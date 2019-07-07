using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Character
{

    Animator animator;

    public LevelSceneManager levelSceneManager;

    // For testing in autopolite mode 
    GameObject targetObject;
    List<GameObject> enemies = new List<GameObject>();    
    SimpleTimer fireRateTimer;
    float fireRateDuration = 0.01f;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        LoadPlayerAndStats();

        // Autopilot mod
        fireRateTimer = gameObject.AddComponent<SimpleTimer>();
    }

    protected override void Update()
    {
        base.Update();
        RecoverShield();
        UpdateBars();

        if (!isGamePaused()) 
        {
            if (PersistentManagerScript.Instance.autopilot)
            {
                health = 100;
                shield = 40;
                if (!isBouncing())
                {
                    ProcessAutoPilotTest();
                    ProcessFireInput();
                    ProcessInput();
                }
            }
            else {
                if (!isBouncing())
                {
                    ProcessInputMovementAndMouseLooking();
                    ProcessFireInput();
                    ProcessInput();
                }
            }
        }
    }

    void LoadPlayerAndStats()
    {
        // Player stats
        health = PersistentManagerScript.Instance.p_startingHealth;   
        shield = PersistentManagerScript.Instance.p_startingShieldCapacity;   
        shieldRecoverTimeDuration = PersistentManagerScript.Instance.p_shieldRecoverTimeDuration;   
        
        bulletSpeed = PersistentManagerScript.Instance.p_bulletSpeed;

        if (PersistentManagerScript.Instance.autopilot) 
            bulletDamage = PersistentManagerScript.Instance.p_bulletDamage;

    }

    void ProcessInput()
    {
    }

    bool isGamePaused()
    {
        return Time.timeScale == 0;
    }

    void ProcessFireInput()
    {
        // Fire Bullet
        if (Input.GetButtonDown("Fire1"))
        {
            LaunchBullet();
        }
        // Fire Rocket
        if (Input.GetButtonDown("Fire2"))
        {
            if (PersistentManagerScript.Instance.p_rocketAmmo > 0)
            {
                PersistentManagerScript.Instance.p_rocketAmmo--;
                LaunchRocket();
            }
        }
    }

    void ProcessInputMovementAndMouseLooking()
    {
        // Input Moving 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        Vector2 position = rigidbody2d.position;
        move *= speed;
        rigidbody2d.velocity = new Vector3(move.x, move.y, 0.0f);

        // Looking at the mouse
        position = position + move * speed * Time.deltaTime;
        var direction = Camera.main.WorldToScreenPoint(position) - Input.mousePosition;

        lookDirection.Set(-direction.x, -direction.y);
        lookDirection.Normalize();
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90.0f;
       // rigidbody2d.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        lookRotation = Quaternion.AngleAxis(angle, Vector3.forward); 
        rigidbody2d.transform.rotation = lookRotation;


        // Eleminate the health bar rotation and position modification
        healthBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        healthBar.transform.position = transform.position + healthBarOffset;
        shieldBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        shieldBar.transform.position = transform.position + shieldBarOffset;
    }

    public void GetCoin()
    {
        levelSceneManager.coinsEarned++;
    }

    public override void GetDestroy()
    {
        PersistentManagerScript.Instance.Loose();
        Destroy(gameObject);
    }

    public override void RecoverShield()
    {
        // Recover Shield
        float p_shieldRecoverRateDuration = PersistentManagerScript.Instance.p_shieldRecoverRateDuration;
        int p_startingShieldCapacity = PersistentManagerScript.Instance.p_startingShieldCapacity;
        
        if (shieldRecoverTimer.isFinished())
        {
            // Small timer to avoid shield instant recover
            if (shieldRecoverRateTimer.isFinished())
            {
                shieldRecoverRateTimer.StartTimer(p_shieldRecoverRateDuration);
            }
            else
            {
                shield = Mathf.Clamp(shield + 1, 0, p_startingShieldCapacity);
            }
        }    

    }

    void UpdateBars()
    {
        healthBar.SetSize((float)health/(float)PersistentManagerScript.Instance.p_startingHealth);
        shieldBar.SetSize((float)shield/(float)PersistentManagerScript.Instance.p_startingShieldCapacity);
    }

    // TESTING PURPOSE 
    // Autopilote mod
    void ProcessAutoPilotTest()
    {
        // Update enemies list
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        enemies.Remove(gameObject);

        // Find closest enemy
        if (enemies.Count > 0) {
            targetObject = FindClosestEnemy();

            // Make enemy look at the target
            Vector2 direction = new Vector2(targetObject.transform.position.x,
                    targetObject.transform.position.y) - rigidbody2d.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rigidbody2d.transform.rotation = Quaternion.AngleAxis(angle - 90.0f,
                    Vector3.forward);
            lookDirection.Set(direction.x, direction.y);
            lookDirection.Normalize();

            // Make enemy going towards the target
            rigidbody2d.velocity = new Vector3(direction.x * speed, direction.y * speed, 0.0f);

            // Eleminate the health bar rotation and position modification
            healthBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            healthBar.transform.position = transform.position + healthBarOffset;
            shieldBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            shieldBar.transform.position = transform.position + shieldBarOffset;
            // Fire Bullet
            if (fireRateTimer.isFinished())
            {
                fireRateTimer.StartTimer(fireRateDuration);
                LaunchBullet();
            }
        }
    }

    public GameObject FindClosestEnemy()
    {
        float distanceToClosestEnemy = float.MaxValue;
        GameObject closestEnemy = null;

        foreach(GameObject currentEnemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(currentEnemy.transform.position, transform.position);
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        } 

        return closestEnemy;
    }

    public override bool isPlayer()
    {
        return true;
    }

}
