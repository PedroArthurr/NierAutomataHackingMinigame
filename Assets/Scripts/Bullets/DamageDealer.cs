using UnityEngine.Events;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private bool bullet = true;
    [SerializeField] private LayerMask targetLayers;

    public DamageableEvent OnDamageableEnter;
    public UnityEvent OnDamageableExit;

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage();
                if (bullet)
                    Destroy(gameObject);

                OnDamageableEnter?.Invoke(damageable);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Notify listeners when object leaves the trigger
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
            OnDamageableExit?.Invoke();
    }
}