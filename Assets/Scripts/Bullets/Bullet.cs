using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(DamageDealer))]
public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    [FormerlySerializedAs("ignoreLayers")]
    public LayerMask interactionLayers;

    [SerializeField] private Rigidbody rb;

    [SerializeField] private DamageDealer damageDealer;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (damageDealer == null)
            damageDealer = GetComponent<DamageDealer>();
    }

    void Start() => rb.velocity = transform.forward * speed;

    //void Update()
    //{
    //    if (!GetComponent<Renderer>().isVisible)
    //        Destroy(gameObject);
    //}

    public IEnumerator DelayedDestroy()
    {
        yield return new WaitForEndOfFrame();
        if (gameObject != null)
            Destroy(gameObject);
    }
}