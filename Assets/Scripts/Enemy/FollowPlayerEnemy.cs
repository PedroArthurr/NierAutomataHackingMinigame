using UnityEngine;

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

        Vector3 targetPos = new(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z);
        Vector3 direction = (targetPos - transform.position);
        LookAtPlayer(direction);
    }

    protected override void Move()
    {
        if (playerTransform == null)
            return;

        Vector3 targetPos = new(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z);
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        if (distanceToTarget > stoppingDistance)
        {
            moveDirection = (targetPos - transform.position).normalized;

            if (Physics.Raycast(transform.position, moveDirection, out RaycastHit hit, 1f, obstacleLayers))
                moveDirection = Vector3.Reflect(moveDirection, hit.normal);

            transform.position += speed * Time.deltaTime * moveDirection;

        }
        Vector3 direction = (targetPos - transform.position);
        direction.y = 0;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        if (rotationSpeed != 0)
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        else
            LookAtPlayer(direction);

    }

    private void LookAtPlayer(Vector3 direction)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
}