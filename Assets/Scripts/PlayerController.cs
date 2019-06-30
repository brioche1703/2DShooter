using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : Character
{

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (bouncingTimer <= 0.0f)
        {
            ProcessInputMovementAndMouseLooking();
            ProcessOtherInputs();
        }
    }

    void ProcessOtherInputs()
    {
        // Fire Bullet
        if (Input.GetButtonDown("Fire1"))
        {
            Launch();
        }
    }

    void ProcessInputMovementAndMouseLooking()
    {
        // Input Moving 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        Vector2 position = rigidbody2d.position;
        position = position + move * speed * Time.deltaTime;
        move *= speed;
        rigidbody2d.velocity = new Vector3(move.x, move.y, 0.0f);

        // Looking at the mouse
        var direction = Camera.main.WorldToScreenPoint(position) - Input.mousePosition;
        lookDirection.Set(-direction.x, -direction.y);
        lookDirection.Normalize();
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rigidbody2d.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Eleminate the health bar rotation and position modification
        healthBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        healthBar.transform.position = transform.position + healthBarOffset;
        shieldBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        shieldBar.transform.position = transform.position + shieldBarOffset;
    }


    public void Launch()
    {
        GameObject bulletObject = Instantiate(bulletPrefab, rigidbody2d.position + lookDirection * new Vector2(0.5f, 0.5f), Quaternion.identity);
        BulletController bullet = bulletObject.GetComponent<BulletController>();
        bullet.SetLauncherGameObject(this);
        bullet.Launch(lookDirection, 300);
    }
    
    public void GetCoin()
    {
        PersistentManagerScript.Instance.coinsNumber++;
        Debug.Log(PersistentManagerScript.Instance.coinsNumber);
    }

    public override void GetDestroy()
    {
        Destroy(gameObject);
        PersistentManagerScript.Instance.Loose();
    }
}
