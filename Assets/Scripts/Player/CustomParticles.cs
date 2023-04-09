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

        GameObject particlePrefab = particlePrefabs[Random.Range(0, particlePrefabs.Length)];
        GameObject particle = null;
        
        // Check if there's an available particle in the pool
        for (int i = 0; i < particles.Count; i++)
        {
            if (!particles[i].activeInHierarchy)
            {
                particle = particles[i];
                break;
            }
        }

        // If there's no available particle, create a new one
        if (particle == null)
        {
            particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            particles.Add(particle);
        }
        float speed = Random.Range(minSpeed, maxSpeed);
        float lifetime = Random.Range(minLifetime, maxLifetime);
        float scale = Random.Range(0f, .25f);

        particle.transform.position = transform.position;
        particle.transform.localEulerAngles = new Vector3 (Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));
        particle.transform.localScale = new Vector3(scale, scale, scale);
        particle.SetActive(true);

        Rigidbody rb = particle.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * speed;

        // Disable the particle after its lifetime has expired
        StartCoroutine(DisableParticle(particle, lifetime));

        particleCount++;
    }

    private IEnumerator DisableParticle(GameObject particle, float lifetime)
    {
        yield return new WaitForSeconds(lifetime);

        // Check if the particle is still active before trying to disable it
        if (particle != null && particle.activeInHierarchy)
        {
            particle.SetActive(false);
            particleCount--;
        }
    }
}
