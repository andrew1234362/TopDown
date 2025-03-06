using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Input Settings")]
    public string dashButton = "Dash";

    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    private bool isDashing = false;
    private bool canDash = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastMoveDirection = Vector2.right; // Default direction
    }

    void Update()
    {
        // Get input only when not dashing
        if (!isDashing)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;

            // Update last moved direction
            if (moveDirection != Vector2.zero)
            {
                lastMoveDirection = moveDirection;
            }
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
		
		
    }

    void FixedUpdate()
    {
        // Apply movement only when not dashing
        if (!isDashing)
        {
            rb.velocity = moveDirection * moveSpeed;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Determine dash direction
        Vector2 dashDirection = moveDirection != Vector2.zero ? moveDirection : lastMoveDirection;
        dashDirection = dashDirection.normalized;

        // Apply dash velocity
        rb.velocity = dashDirection * dashSpeed;

        // Wait for dash duration
        yield return new WaitForSeconds(dashDuration);

        // Reset velocity and dash state
        isDashing = false;
        rb.velocity = moveDirection * moveSpeed;

        // Wait for cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}