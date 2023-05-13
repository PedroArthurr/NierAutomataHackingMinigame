using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleSpinner : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    [SerializeField] private GameObject orbitObjectPrefab;
    public float orbitSpeed = 1.0f;
    [SerializeField] private float orbitRadius = 5.0f;
    [SerializeField] private SpinningShooterEnemy spinningShooterEnemy;
    private List<Transform> orbitObjects = new();

    public Vector3 angularVelocity;
    public Rigidbody rb;
    void Start()
    {
        if (parentObject == null || orbitObjectPrefab == null)
        {
            Debug.LogError("Parent and/or orbit object prefab not set.");
            return;
        }

        for (int i = 0; i < 6; i++)
        {
            float angle = i * Mathf.PI * 2f / 6;
            Vector3 newPos = parentObject.transform.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius;
            GameObject orbitObject = Instantiate(orbitObjectPrefab, newPos, Quaternion.identity);
            orbitObject.transform.parent = parentObject.transform;
            orbitObjects.Add(orbitObject.transform);
        }
        spinningShooterEnemy.Muzzles = orbitObjects;
    }

    void Update()
    {
        for (int i = 0; i < orbitObjects.Count; i++)
        {
            GameObject orbitObject = orbitObjects[i].gameObject;
            if (orbitObject != null)
            {
                orbitObject.transform.RotateAround(parentObject.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
                Vector3 lookDir = (orbitObject.transform.position - parentObject.transform.position).normalized;
                orbitObject.transform.rotation = Quaternion.LookRotation(lookDir);
            }
        }

        angularVelocity = rb.angularVelocity;
    }

}
