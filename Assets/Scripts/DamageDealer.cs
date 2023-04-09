using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private bool bullet = true;

    [SerializeField] private LayerMask targetLayers;
    //public void DealDamage(IDamageable damageable) => damageable.TakeDamage();

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            damageable?.TakeDamage();
            if (bullet)
                Destroy(gameObject);
        }
    }
}