using UnityEngine;

public class FollowPlayerEnemy : Enemy
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float stoppingDistance = 1.5f;
    [SerializeField] private LayerMask obstacleLayers;
    [SerializeField] private float wallSlowdownMultiplier = 0.1f;

    private Vector3 moveDirection;
    private bool isCollidingWithWall = false;

    public Transform PlayerTransform { get => playerTransform; }

    protected override void Start()
    {
        base.Start();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        Vector3 targetPos = new Vector3(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z);
        Vector3 direction = (targetPos - transform.position);
        LookAtPlayer(direction);
    }

    protected override void Move()
    {
        if (playerTransform == null)
            return;

        Vector3 targetPos = new Vector3(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z);
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        if (distanceToTarget > stoppingDistance)
        {
            Vector3 moveDirection = (targetPos - transform.position).normalized;
            float currentSpeed = isCollidingWithWall ? speed * wallSlowdownMultiplier : speed;
            transform.position += currentSpeed * Time.deltaTime * moveDirection;

            if (Physics.Raycast(transform.position, moveDirection, 1f, obstacleLayers))
            {
                isCollidingWithWall = true;
            }
            else
            {
                isCollidingWithWall = false;
            }
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

    public void OnDrawGizmos()
    {
        Gizmos.color = isCollidingWithWall ? Color.red : Color.blue;
        Gizmos.DrawRay(transform.position, moveDirection);
    }
}
