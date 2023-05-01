using UnityEngine;
using UnityEngine.EventSystems;

public class FollowPlayerEnemy : Enemy
{
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private LayerMask obstacleLayers;

    private Vector3 moveDirection;

    public Transform PlayerTransform { get => playerTransform; }

    protected override void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected override void Move()
    {
        if (playerTransform == null)
            return;

        Vector3 targetPos = new(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z);
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        if (distanceToTarget > stoppingDistance)
        {
            // calcula a direção do movimento
            moveDirection = (targetPos - transform.position).normalized;

            // cria um raio para verificar colisões
            if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, 0.5f, obstacleLayers))
            {
                // se o raio atingir um obstáculo, ajusta a direção do movimento
                moveDirection = Vector3.Reflect(moveDirection, hit.normal);
            }

            // move o objeto na direção calculada
            transform.position += Speed * Time.deltaTime * moveDirection;
        }
    }

}