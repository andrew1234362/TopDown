using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRange = 5f;
    public float chaseSpeed = 3f;
    public float stoppingDistance = 1f;
    public LayerMask detectionLayers; // Changed from obstacleLayers

    [Header("References")]
    private Transform player;
    private Rigidbody2D rb;

    [Header("State Management")]
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool hasLineOfSight = false;

        if (distanceToPlayer <= detectionRange)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            
            // Raycast with debug visualization
            RaycastHit2D hit = Physics2D.Raycast(
                transform.position, 
                directionToPlayer, 
                detectionRange, 
                detectionLayers
            );

            Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.red);

            // If we hit something and it's the player
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hasLineOfSight = true;
            }
        }

        isChasing = hasLineOfSight;
        UpdateChaseBehavior();
		
		
    }

    void UpdateChaseBehavior()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void ChasePlayer()
    {
        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            Vector2 moveDirection = (player.position - transform.position).normalized;
            rb.velocity = moveDirection * chaseSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}