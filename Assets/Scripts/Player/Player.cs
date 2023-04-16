using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int baseHealth = 2;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject[] shields;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject core;
    [Space]
    [SerializeField] private PlayerController controller;
    [Space]
    [SerializeField] private GameObject takeDamageParticles;
    [SerializeField] private Color shipColor;
    [SerializeField] private Color coreColor;

    private bool canReceiveDamage = true;
    private float canReceiveDamageTime = 1.5f;

    public int CurrentHealth { get => currentHealth; }

    private void Start()
    {
        if (controller == null)
            controller = GetComponent<PlayerController>();

        CameraFollow.instance.UpdateTarget(this.transform);
        currentHealth = baseHealth;
    }

    public void TakeDamage()
    {
        if (!canReceiveDamage)
            return;
        DestroyShield();
        currentHealth -= 1;
        if (CurrentHealth < 0)
            Die();
    }

    private void DestroyShield()
    {
        if (currentHealth == 2)
            shields[0].SetActive(false);
        else if (currentHealth == 1)
            shields[1].SetActive(false);
        StartCoroutine(ImmortalTime());

        StartCoroutine(FadeToBlack(bow));
        //StartCoroutine(FadeToBlack(coreColor''));
        if (shields[1] != null)
            StartCoroutine(FadeToBlack(shields[1]));
    }

    private IEnumerator ImmortalTime()
    {
        canReceiveDamage = false;
        yield return new WaitForSeconds(canReceiveDamageTime);
        canReceiveDamage = true;
    }

    private void Die()
    {
        controller.CanMove = false;
    }

    private IEnumerator FadeToBlack(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        float duration = 0.2f;
        float startTime = Time.time;
        takeDamageParticles.SetActive(true);

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            foreach (Renderer r in renderers)
            {
                r.material.color = Color.Lerp(shipColor, Color.black, t);
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            foreach (Renderer r in renderers)
            {
                r.material.color = Color.Lerp(Color.black, shipColor, t);
            }
            yield return null;
        }
        takeDamageParticles.SetActive(false);
    }
}
