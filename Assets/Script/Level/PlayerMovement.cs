using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public float jumpCutMultiplier = 0.5f; // 0.5 = cut height by half on release

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontalMovement;

    void FixedUpdate()
    {
        // Unity 6: Use 'linearVelocity' instead of 'velocity'
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        // 1. Start Jump (Button Pressed)
        if (context.performed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
        }

        // 2. Low Jump (Button Released Early)
        // If we release the button AND we are still moving up...
        if (context.canceled && rb.linearVelocity.y > 0f)
        {
            // ...cut the vertical speed by our multiplier
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
    }

    // Simple helper to check if we are on the ground
    private bool IsGrounded()
    {
        // Creates a small circle at the feet to check for ground collision
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}