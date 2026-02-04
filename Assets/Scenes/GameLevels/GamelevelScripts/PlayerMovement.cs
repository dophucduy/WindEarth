using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // <--- ADDED: Needed to reload the scene

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public float jumpCutMultiplier = 0.5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontalMovement;

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }

        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }

    // --- NEW SECTION: DEATH LOGIC ---
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If we hit an object tagged as "Trap", we die
        if (collision.gameObject.CompareTag("Trap"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Reloads the current scene to restart
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // --------------------------------

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
}