using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : FollowPlayerEnemy
{
    public GameObject bulletPrefab;

    private float lastShootTime = 0f;
    protected override void Shoot()
    {
        if (PlayerTransform == null)
            return;

        if (Time.time - lastShootTime >= ShootInterval)
        {
            Vector3 directionToPlayer = (PlayerTransform.position - transform.position).normalized;
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, rotationToPlayer);
            lastShootTime = Time.time;
        }
    }
}
