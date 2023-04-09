using UnityEngine;

public class ParticleBillboard : MonoBehaviour
{
    [SerializeField] private float maxRotationOffset = 10f; 
    private Transform cam;

    private void Start() => cam = Camera.main.transform;

    private void OnEnable()
    {
        float randomOffset = Random.Range(-maxRotationOffset, maxRotationOffset);
        transform.Rotate(new Vector3(0f, 0f, randomOffset));
    }

    private void LateUpdate() => transform.LookAt(transform.position + cam.forward);
}
