using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DamageDealer))]
public class DamageWall : MonoBehaviour
{
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private DamageDealer damageDealer;
    [SerializeField] private float damageDelay = 1.5f;

    private Coroutine damageCoroutine;
    private IDamageable damageTarget;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (damageDealer == null)
            damageDealer = GetComponent<DamageDealer>();

        damageDealer.OnDamageableEnter.AddListener(target => StartDamage(target));
        damageDealer.OnDamageableExit.AddListener(StopDamage);
    }

    public void StartDamage(IDamageable target)
    {
        damageTarget = target;
        damageCoroutine = StartCoroutine(DamageOverTime());
    }

    public void StopDamage()
    {
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageTarget = null;
    }

    private IEnumerator DamageOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageDelay);
            damageTarget?.TakeDamage();
        }
    }
}