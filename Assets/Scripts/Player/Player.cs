using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private int baseHealth = 2;
    [SerializeField] private int currentHealth;
    [SerializeField] private GameObject[] shields;
    [Space]
    [SerializeField] private PlayerController controller;

    private bool canReceiveDamage = true;
    private float immortalTime = .6f;

    public int CurrentHealth { get => currentHealth; }

    private void Start()
    {
        if(controller ==  null)
            controller = GetComponent<PlayerController>();

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
    }

    private IEnumerator ImmortalTime()
    {
        canReceiveDamage = false;
        yield return new WaitForSeconds(immortalTime);
        canReceiveDamage = true;
    }

    private void Die()
    {
        controller.CanMove = false;
    }
}
