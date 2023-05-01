using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodBurstAnimation : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfParticles = 40;
    public float minScale = 0.1f; 
    public float maxScale = 0.5f; 
    public float lifetime = 2f;

    void OnEnable()
    {
        Vector3 origin = transform.position;

        for (int i = 0; i < numberOfParticles; i++)
        {
            GameObject particle = Instantiate(prefab, origin, Quaternion.identity);

            Vector3 position = Random.onUnitSphere;
            if (position.y < 0)
            {
                position.y = -position.y;
            }

            float angle = Vector3.Angle(position, Vector3.up);
            {
                position.y = Mathf.Abs(position.y);
                angle = Vector3.Angle(position, Vector3.up);
            }
            Vector3 axis = Vector3.Cross(position, Vector3.up);
            particle.transform.Rotate(axis, angle, Space.World);

         
            float scale = Random.Range(minScale, maxScale);
            particle.transform.localScale = new Vector3(scale, scale, scale);

            Destroy(particle, lifetime);
        }
    }
}