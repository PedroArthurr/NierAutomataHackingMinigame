using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int baseHealth = 2;
    [SerializeField] private int currentHealth;

    [SerializeField] private float speed = 5f;

    private Type type = Type.Shooter;

    protected Rigidbody rb;

    public int BaseHealth { get => baseHealth; set => baseHealth = value; }
    public float Speed { get => speed; }
    public Type Type { get => type; }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = baseHealth;
    }

    protected virtual void Update()
    {
        Move();
        if (Type == Type.Shooter)
        {
            Shoot();
        }
    }

    protected virtual void Move()
    { }

    protected virtual void Shoot()
    { }

    public void TakeDamage()
    {
        currentHealth -= 1;
        print(currentHealth.ToString());
        if (currentHealth == 1)
            Die();
    }

    private void Die()
    {

    }
}
public enum Type
{
    Shooter,
    Stalker,
    Boss
}