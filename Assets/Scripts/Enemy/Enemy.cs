using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("General Settings")]
    [SerializeField] private int baseHealth = 10;

    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float rotationSpeed = 0f;

    [Header("References")]
    [SerializeField] private GameObject damageParticle;
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private EnemyShield shield;

    [Space]
    [Header("Sounds")]
    [SerializeField] protected string shotSound = "enemy_shoot";
    [SerializeField] protected string hitSound = "enemy_hit";
    [SerializeField] protected string deathSound = "enemy_explode0";
    [SerializeField] protected string deathSound2 = "enemy_explode";

    [Space]
    [Header("Flash Settings")]
    [SerializeField] private Color flashColor;
    [SerializeField] private float flashTime = 0.05f;
    private Material enemyMaterial;
    private bool flashing;
    [Space]

    private Enums.EnemyType type = Enums.EnemyType.Shooter;
    private int currentHealth;

    private bool canMove = true;
    private bool canShoot = true;

    protected AudioClip shot;
    protected AudioClip hit;

    protected Rigidbody rb;

    public int BaseHealth { get => baseHealth; set => baseHealth = value; }
    public Enums.EnemyType Type { get => type; }

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = baseHealth;
        hit = AudioManager.instance.sounds.GetAudioClip(hitSound);
        shot = AudioManager.instance.sounds.GetAudioClip(shotSound);
    }

    protected virtual void Update()
    {
        if (canMove)
            Move();
        if (Type == Enums.EnemyType.Shooter && canShoot)
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
        if (shield != null)
            return;

        SendMessage("OnTakeDamage", SendMessageOptions.DontRequireReceiver);

        AudioManager.instance.PlaySound(hit, .4f);
        currentHealth -= 1;
        if (currentHealth == 0)
            StartCoroutine(Die());

        if (damageParticle)
        {
            var p = Instantiate(damageParticle, this.transform);
            p.transform.position = new Vector3(transform.position.x, transform.position.y - .5f, transform.position.z);
            Destroy(p, .5f);
        }
        StartCoroutine(FlashColor());
    }

    private IEnumerator FlashColor()
    {
        if (enemyMaterial == null)
            enemyMaterial = GetComponent<MeshRenderer>().material;

        if (flashing)
            yield break;

        flashing = true;
        Color currentEmissionColor = enemyMaterial.GetColor("_EmissionColor");
        float elapsedTime = 0f;
        float duration = 0.1f;

        while (elapsedTime < flashTime)
        {
            float t = elapsedTime / duration;
            enemyMaterial.SetColor("_EmissionColor", Color.Lerp(currentEmissionColor, flashColor * Mathf.Pow(2, 2), t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < flashTime / 2)
        {
            float t = elapsedTime / duration;
            enemyMaterial.SetColor("_EmissionColor", Color.Lerp(flashColor * Mathf.Pow(2, 2), currentEmissionColor, t));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemyMaterial.SetColor("_EmissionColor", currentEmissionColor);
        flashing = false;
    }

    private IEnumerator Die()
    {
        canMove = false;
        canShoot = false;
        if (deathParticle != null)
        {
            GameManager.instance.enemiesController.OnDestroyEnemy(this);
            var p = Instantiate(deathParticle);
            p.transform.position = this.transform.position;
            Destroy(p, 1);

            yield return new WaitForSeconds(.2f);
        }

        AudioManager.instance.PlaySound(AudioManager.instance.sounds.GetAudioClip(deathSound2), .3f);

        Destroy(this.gameObject);
    }

    public void DestroyShield()
    {
        if (shield != null)
            shield.Remove();
    }
}
