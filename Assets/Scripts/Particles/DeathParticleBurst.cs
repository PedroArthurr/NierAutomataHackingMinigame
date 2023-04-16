using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleBurst : MonoBehaviour
{
    [SerializeField] private GameObject[] particlePrefabs;
    [SerializeField] private int objectsPerIteration;
    [SerializeField] private int iterations;
    [SerializeField] private float initialRadius;
    [SerializeField] private float finalRadius;
    [SerializeField] private float minLifetime = 0.2f;
    [SerializeField] private float maxLifetime = 1f;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private float minSize = 0.1f;
    [SerializeField] private float maxSize = 0.3f;

    private IEnumerator BurstCoroutine()
    {
        for (int i = 0; i < iterations; i++)
        {
            float radius = Mathf.Lerp(initialRadius, finalRadius, (float)i / iterations);
            List<GameObject> particles = new List<GameObject>();
            for (int j = 0; j < objectsPerIteration; j++)
            {
                Vector3 position = transform.position + Random.insideUnitSphere * radius;
                position.y = Mathf.Abs(position.y);
                Vector3 rotation = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

                GameObject particlePrefab = particlePrefabs[Random.Range(0, particlePrefabs.Length)];
                GameObject particle = Instantiate(particlePrefab, position, Quaternion.identity);
                particle.transform.parent = transform;
                particles.Add(particle);

                Renderer renderer = particle.GetComponent<Renderer>();
                Material material = renderer.material;
                float size = Random.Range(minSize, maxSize);
                particle.transform.localScale = new Vector3(size, size, size);
                particle.transform.localEulerAngles = rotation;
            }

            float elapsedTime = 0f;
            float lifetime = Random.Range(minLifetime, maxLifetime);

            while (elapsedTime < lifetime)
            {
                float fadeNormalizedTime = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01((lifetime - elapsedTime) / fadeDuration));
                Color color = particles[0].GetComponent<Renderer>().material.color;
                color.a = fadeNormalizedTime;
                foreach (GameObject particle in particles)
                    particle.GetComponent<Renderer>().material.color = color;
                yield return null;
                elapsedTime += Time.deltaTime;
            }

            foreach (GameObject particle in particles)
                Destroy(particle);

            yield return new WaitForEndOfFrame();
        }
    }

    public void Burst()
    {
        StartCoroutine(BurstCoroutine());
    }
}
