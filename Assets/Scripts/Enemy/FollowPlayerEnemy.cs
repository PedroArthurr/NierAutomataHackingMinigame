using UnityEngine;

public class FollowPlayerEnemy : Enemy
{
    
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float stoppingDistance = 1.5f;
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

        Vector3 targetPos = new Vector3(PlayerTransform.position.x, transform.position.y, PlayerTransform.position.z);
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);

        if (distanceToTarget > stoppingDistance) 
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Speed * Time.deltaTime);
    }

}