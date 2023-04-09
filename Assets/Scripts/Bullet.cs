using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public LayerMask ignoreLayers;
    public LayerMask damageLayer;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private DamageDealer damageDealer;

    void Start() => rb.velocity = transform.forward * speed;

    void Update()
    {
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator DelayedDestroy()
    {
        yield return new WaitForEndOfFrame();
        if (gameObject != null)
            Destroy(gameObject);
    }
}