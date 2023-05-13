using System.Collections.Generic;
using UnityEngine;

public class SpinningShooterEnemy : ShooterEnemy
{
    [SerializeField] protected MuzzleSpinner muzzleSpinner;
    [SerializeField] private List<Transform> muzzles = new();
    private Enums.BulletTypes currentBulletType = Enums.BulletTypes.PURPLE;
    public List<Transform> Muzzles { set => muzzles = value; }

    protected override void Shoot()
    {
        if (Time.time - lastShootTime >= shootInterval)
        {
            for (int i = 0; i < muzzles.Count; i++)
            {
                Transform muzzle = muzzles[i];
                Vector3 muzzleDirection = muzzle.forward;
                Quaternion rotationToPlayer = Quaternion.LookRotation(muzzleDirection, Vector3.up);

                // Determine the bullet type based on the index of the muzzle.
                Enums.BulletTypes muzzleBulletType;
                switch (bulletType)
                {
                    case Enums.BulletTypes.MIXED:
                        muzzleBulletType = (i % 2 == 0) ? Enums.BulletTypes.PURPLE : Enums.BulletTypes.ORANGE;
                        break;
                    default:
                        muzzleBulletType = bulletType;
                        break;
                }

                GameObject bullet = Instantiate(bulletPrefabs[(int)muzzleBulletType], muzzle.position, rotationToPlayer);
                Bullet bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    // Calculate the radial distance from the muzzle to the center of rotation.
                    float radialDistance = Vector3.Distance(muzzle.position, transform.position);

                    // Calculate the linear velocity of the muzzle.
                    Vector3 muzzleVelocity = radialDistance * Mathf.Deg2Rad * muzzleSpinner.orbitSpeed * -muzzleDirection;

                    bulletScript.initialVelocity = muzzleVelocity;
                }
            }

            lastShootTime = Time.time;
            AudioManager.instance.PlaySound(AudioManager.instance.sounds.GetAudioClip(shotSound), .1f);
        }
    }

}
