using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRange = 5f;
    public float chaseSpeed = 3f;
    public float stoppingDistance = 1f;
    public LayerMask obstacleLayers;

    [Header("References")]
    private Transform player;
    private Rigidbody2D rb;

    [Header("State Management")]
    public bool isChasing = false;
    private Vector2 lastKnownPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Calculate direction to player
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            
            // Raycast to check line of sight
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, obstacleLayers);
            
            // Debug visualization
            Debug.DrawRay(transform.position, directionToPlayer * detectionRange, Color.red);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                // Player is visible
                isChasing = true;
                lastKnownPosition = player.position;
            }
            else
            {
                // Player is obstructed or out of range
                isChasing = false;
            }
        }
        else
        {
            isChasing = false;
        }

        // Handle chasing behavior
        if (isChasing == true)
        {
            ChasePlayer();
        }
        else
        {
            // Optional: Add idle behavior here
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
		isChasing = true;
    }
}
