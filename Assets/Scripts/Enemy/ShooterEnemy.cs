using UnityEngine;

public class ShooterEnemy : FollowPlayerEnemy
{
    [SerializeField] protected Enums.ShootingBehaviour shootingBehaviour;
    [SerializeField] protected Enums.BulletTypes bulletType;
    [SerializeField] protected GameObject[] bulletPrefabs;
    [SerializeField] protected float shootInterval = 1f;

    protected float lastShootTime = 0f;

    protected override void Shoot()
    {
        if (PlayerTransform == null)
            return;
        if (Time.time - lastShootTime >= shootInterval)
        {
            Vector3 directionToPlayer = (PlayerTransform.position - transform.position).normalized;
            Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            switch (shootingBehaviour)
            {
                case Enums.ShootingBehaviour.SEMIAUTO:
                case Enums.ShootingBehaviour.MACHINEGUN:
                    Instantiate(bulletPrefabs[(int)bulletType], transform.position, rotationToPlayer);
                    lastShootTime = Time.time;
                    break;

                case Enums.ShootingBehaviour.SHOTGUN:
                    int numberOfBullets = 3;
                    float[] angles = { -15f, 0f, 15f };
                    for (int i = 0; i < numberOfBullets; i++)
                    {
                        Quaternion rotation = Quaternion.Euler(0f, angles[i], 0f) * rotationToPlayer;
                        Instantiate(bulletPrefabs[(int)bulletType], transform.position, rotation);
                    }
                    lastShootTime = Time.time;
                    break;
            }
            AudioManager.instance.PlaySound(AudioManager.instance.sounds.GetAudioClip(shotSound), .1f);
        }
    }
}
