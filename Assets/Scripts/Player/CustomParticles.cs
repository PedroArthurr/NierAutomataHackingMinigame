using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomParticles : MonoBehaviour
{
    [SerializeField] private GameObject[] particlePrefabs;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxLifetime = 5f;
    [SerializeField] private float minLifetime = 1f;
    [SerializeField] private float spawnRate = 0.1f;
    [SerializeField] private int maxParticles = 100;

    public bool isMoving;
    public bool alive;

    private float nextSpawnTime;
    private int particleCount;
    private List<GameObject> particles = new List<GameObject>();

    // Define a range for the offset of the spawn position
    [SerializeField] private float spawnOffsetRange = 0.2f;

    private void Update()
    {
        if (particleCount >= maxParticles)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;
            SpawnParticle();
        }
    }

    private void SpawnParticle()
    {
        if (!alive)
            return;

        if (!isMoving)
            return;

        int numParticles = Random.Range(1, 4); // Select a random number of particles to spawn (1 to 3)

        for (int i = 0; i < numParticles; i++)
        {
            GameObject particlePrefab = particlePrefabs[Random.Range(0, particlePrefabs.Length)];
            GameObject particle = null;

            // Check if there's an available particle in the pool
            for (int j = 0; j < particles.Count; j++)
            {
                if (!particles[j].activeInHierarchy)
                {
                    particle = particles[j];
                    break;
                }
            }

            // If there's no available particle, create a new one
            if (particle == null)
            {
                particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                particles.Add(particle);
            }

            // Define an offset position for the particle
            Vector3 spawnOffset = new Vector3(Random.Range(-spawnOffsetRange, spawnOffsetRange), 0f, Random.Range(-spawnOffsetRange, spawnOffsetRange));

            // Set the position and rotation of the particle
            particle.transform.position = transform.position + spawnOffset;
            //particle.transform.localEulerAngles = new Vector3(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

            // Set the scale of the particle
            float scale = Random.Range(0f, .25f);
            particle.transform.localScale = new Vector3(scale, scale, scale);

            // Activate the particle
            particle.SetActive(true);

            // Set the velocity of the particle
            float speed = Random.Range(minSpeed, maxSpeed);
            Rigidbody rb = particle.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * speed;

            // Disable the particle after its lifetime has expired
            StartCoroutine(DisableParticle(particle, Random.Range(minLifetime, maxLifetime)));
        }

        particleCount += numParticles;
    }
    private IEnumerator DisableParticle(GameObject particle, float lifetime)
    {
        float fadeOutTime = 1.0f; // tempo de fade out em segundos
        float t = 0.0f;
        Material mat = particle.GetComponent<Renderer>().material;
        Color originalColor = mat.color;

        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            float normalizedTime = t / fadeOutTime;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Lerp(1, 0, normalizedTime));
            mat.color = newColor;
            yield return null;
        }

        // desativa a partícula
        particle.SetActive(false);
        particleCount--;
    }
}