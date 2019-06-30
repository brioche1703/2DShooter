using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{

    public float fireRate = 160.0f;
    public float fireRateTimer;

    public GameObject coinPrefab;

    GameObject targetObject;

    List<GameObject> enemies = new List<GameObject>();    

    protected override void Start()
    {
        base.Start();
        fireRateTimer = 0.1f;
    }

    protected override void Update()
    {
        base.Update();
        if (bouncingTimer <= 0.0f)
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
        if (enemies.Count == 0)
            PersistentManagerScript.Instance.Loose();

        // Find closest enemy
        targetObject = FindClosestEnemy();

        // Make enemy look at the target
        Vector2 direction = new Vector2(targetObject.transform.position.x,
                targetObject.transform.position.y) - rigidbody2d.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rigidbody2d.transform.rotation = Quaternion.AngleAxis(angle - 90.0f,
                Vector3.forward);
        lookDirection.Set(-direction.x, -direction.y);
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
        fireRateTimer -= Time.deltaTime;
        if (fireRateTimer < 0)
        {
            fireRateTimer = fireRate;
            Launch();
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

    public void Launch()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, rigidbody2d.position - lookDirection * new Vector2(0.5f, 0.5f), Quaternion.identity);
        BulletController bullet = bulletObject.GetComponent<BulletController>();
        bullet.SetLauncherGameObject(this);
        bullet.Launch(-lookDirection, 100);
    }

    void RealeaseCoin()
    {
        Instantiate(coinPrefab, rigidbody2d.position, Quaternion.identity);
    }

    public override void GetDestroy()
    {
        Destroy(gameObject);
        PersistentManagerScript.Instance.enemiesNumber--;
        RealeaseCoin();
    }
}
