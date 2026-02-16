using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Player Selection")]
    public bool isPlayer1 = true; // Player 1 uses AWD, Player 2 uses Arrow Keys

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public float jumpCutMultiplier = 0.5f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontalMovement;
    private bool jumpPressed;
    private bool jumpReleased;

    void Start()
    {
        // Auto-assign Rigidbody2D if not set
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("PlayerMovement: No Rigidbody2D found! Please add a Rigidbody2D component.");
            }
        }

        // Auto-create ground check if not set
        if (groundCheck == null)
        {
            GameObject groundCheckObj = new GameObject("GroundCheck");
            groundCheckObj.transform.SetParent(transform);
            groundCheckObj.transform.localPosition = new Vector3(0, -0.5f, 0); // Adjust based on your sprite size
            groundCheck = groundCheckObj.transform;
            Debug.Log("PlayerMovement: Auto-created GroundCheck child object.");
        }

        // Set default ground layer if not set
        if (groundLayer.value == 0)
        {
            groundLayer = LayerMask.GetMask("Default");
            Debug.LogWarning("PlayerMovement: groundLayer not set, using 'Default' layer. Set it properly in Inspector for better control.");
        }

        // Wind player (player1) jumps higher by design
        if (isPlayer1)
        {
            jumpPower = jumpPower * 2f;
        }
    }

    void Update()
    {
        // Read keyboard input based on player
        if (isPlayer1)
        {
            // Player 1: A and D for movement, W for jump
            horizontalMovement = 0f;
            if (Input.GetKey(KeyCode.A))
                horizontalMovement = -1f;
            if (Input.GetKey(KeyCode.D))
                horizontalMovement = 1f;

            if (Input.GetKeyDown(KeyCode.W))
                jumpPressed = true;
            if (Input.GetKeyUp(KeyCode.W))
                jumpReleased = true;
        }
        else
        {
            // Player 2: Left and Right arrows for movement, Up arrow for jump
            horizontalMovement = 0f;
            if (Input.GetKey(KeyCode.LeftArrow))
                horizontalMovement = -1f;
            if (Input.GetKey(KeyCode.RightArrow))
                horizontalMovement = 1f;

            if (Input.GetKeyDown(KeyCode.UpArrow))
                jumpPressed = true;
            if (Input.GetKeyUp(KeyCode.UpArrow))
                jumpReleased = true;
        }
    }

    void FixedUpdate()
    {
        // Move the player
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);

        // Handle jump
        if (jumpPressed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpPressed = false;
        }

        // Handle jump cut
        if (jumpReleased && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            jumpReleased = false;
        }
    }

    // --- DEATH LOGIC ---
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
        // Try to let a PlayerLives component handle respawn; fallback to immediate game over
        var lives = GetComponent<PlayerLives>();
        if (lives != null)
        {
            lives.HandleDeath(isPlayer1);
            return;
        }

        LevelFinishManager.Instance.GameOver();
    }

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