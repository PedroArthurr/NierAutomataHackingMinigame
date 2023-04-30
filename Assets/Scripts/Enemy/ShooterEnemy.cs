using UnityEngine;

public class ShooterEnemy : FollowPlayerEnemy
{
    [SerializeField] private ShootingBehaviour shootingBehaviour;
    [SerializeField] private BulletTypes bulletType;
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField] private float shootInterval = 1f;

    private float lastShootTime = 0f;

    protected override void Shoot()
    {
        if (PlayerTransform == null)
            return;

        //AudioManager.instance.PlaySound(AudioManager.instance.sounds.GetAudioClip(shotSound), .6f);

        switch (shootingBehaviour)
        {
            case ShootingBehaviour.SEMIAUTO:
            case ShootingBehaviour.MACHINEGUN:
                if (Time.time - lastShootTime >= shootInterval)
                {
                    Vector3 directionToPlayer = (PlayerTransform.position - transform.position).normalized;
                    Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer, Vector3.up);
                    Instantiate(bulletPrefabs[(int)bulletType], transform.position, rotationToPlayer);
                    lastShootTime = Time.time;
                }
                break;

            case ShootingBehaviour.SHOTGUN:
                if (Time.time - lastShootTime >= shootInterval)
                {
                    Vector3 directionToPlayer = (PlayerTransform.position - transform.position).normalized;
                    Quaternion rotationToPlayer = Quaternion.LookRotation(directionToPlayer, Vector3.up);
                    int numberOfBullets = 3;
                    float[] angles = { -15f, 0f, 15f };
                    for (int i = 0; i < numberOfBullets; i++)
                    {
                        Quaternion rotation = Quaternion.Euler(0f, angles[i], 0f) * rotationToPlayer;
                        Instantiate(bulletPrefabs[(int)bulletType], transform.position, rotation);
                    }
                    lastShootTime = Time.time;
                }
                break;
        }
    }
}

public enum ShootingBehaviour
{
    SEMIAUTO,
    MACHINEGUN,
    SHOTGUN,
}

public enum BulletTypes
{
    PURPLE,
    ORANGE,
}
