using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    SimpleTimer fireRateTimer;
    float fireRateDuration = 2.0f;

    public GameObject coinPrefab;

    GameObject targetObject;

    List<GameObject> enemies = new List<GameObject>();    

    public LevelSceneManager levelSceneManager;

    protected override void Start()
    {
        base.Start();

        fireRateTimer = gameObject.AddComponent<SimpleTimer>();
    }

    protected override void Update()
    {
        // Needs to call GetDestroy here because if multiple collisions
        // happen at the same time it will call GetDestroy too fast
        base.Update();
        RecoverShield();
        UpdateBars();

        if (!isBouncing())
        {
            ProcessMovement();
            ProcessFire();
        }
    }

    void ProcessMovement()
    {
        // Update enemies list
        enemies.Clear();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        enemies.Remove(gameObject);

        // Find closest enemy
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
    }

    void ProcessFire()
    {
        // Fire Bullet
        if (fireRateTimer.isFinished())
        {
            fireRateTimer.StartTimer(fireRateDuration);
            LaunchBullet();
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

    void RealeaseCoin()
    {
        Instantiate(coinPrefab, rigidbody2d.position, Quaternion.identity);
    }

    public override void GetDestroy()
    {
        if (isLastEnemyDamageFromPlayer)
        {
            PersistentManagerScript.Instance.enemiesInLevelKilled++;
            PersistentManagerScript.Instance.score += 10;
        }
        RealeaseCoin();
        PersistentManagerScript.Instance.enemiesDead++;
        Destroy(gameObject);
    }

    void UpdateBars()
    {
        healthBar.SetSize((float)health/(float)startingHealth);
        shieldBar.SetSize((float)shield/(float)startingShield);
    }

    public override bool isPlayer()
    {
        return false;
    }
}
