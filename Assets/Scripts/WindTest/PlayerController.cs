using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SmashMovement : NetworkBehaviour 
{
    [SerializeField] private CharacterType characterType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float fallGravityMultiplier;
    [SerializeField] private float maxFallSpeed;
    public Animator animator;
    private bool isDead = false;
    
    [SerializeField] private bool isPushing;
    [SerializeField] private float pushRange = 1f;
    [SerializeField] private float pushForce = 5f;
    [SerializeField] private LayerMask pushLayer;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;

    private PlayerControls controls;
    private Vector2 moveInput;
    private Vector3 defaultScale;
    private int jumpCount = 0;
    private int maxJump = 1;

    private void Awake()
    {

        animator = GetComponent<Animator>();

        controls = new PlayerControls();
        
        controls.Gameplay.Jump.started += context => Jump();
        controls.Gameplay.Push.started += context => isPushing = true;
        controls.Gameplay.Push.canceled += context => isPushing = false;

        defaultScale = transform.localScale;

    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            controls.Enable();
        }
        Debug.Log("Character: " + characterType);

        if (characterType == CharacterType.Wind) maxJump = 2;
        else maxJump = 1;
    }

    public override void OnNetworkDespawn()
    {
        if (controls != null)
        {
            controls.Disable();
        }
        Debug.Log("Character: " + characterType + " maxJump: " + maxJump);
    }

    void Update()
    {
        if (IsOwner)
        {
            moveInput = controls.Gameplay.Move.ReadValue<Vector2>();

            if (moveInput.x > 0)
                transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
            else if (moveInput.x < 0)
                transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
        }

        bool running = Mathf.Abs(rb.linearVelocity.x) > 0.1f;

        if (IsOwner)
        {
            SetRunningServerRpc(running);
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        //rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        //Debug.Log("Velocity: " + rb.linearVelocity);
        if (Mathf.Abs(moveInput.x) > 0.01f)
        {
            rb.linearVelocity = new Vector2(
                moveInput.x * moveSpeed,
                rb.linearVelocity.y
            );
        }
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && rb.linearVelocity.y <= 0)
        {
            jumpCount = 0;
        }

        if (isPushing)
        {
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

            if (isPushing)
            {
                PushObject();
            }
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f && !isGrounded);
        Debug.Log("Grounded: " + isGrounded + " JumpCount: " + jumpCount);
        // tăng tốc độ rơi
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallGravityMultiplier - 1) * Time.fixedDeltaTime;
        }

        // giới hạn tốc độ rơi
        if (rb.linearVelocity.y < maxFallSpeed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, maxFallSpeed);
        }

    }

    void Jump()
    {
        if (!IsOwner) return;

        if (jumpCount < maxJump)
        {
            Debug.Log("jump force: " + jumpForce);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }
    }

    void PushObject()
    {
        float facingDir = transform.localScale.x;
        Vector2 pushDir = new Vector2(facingDir, 0);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, pushDir, pushRange, pushLayer);
        
        if (hit.collider != null)
        {
            Rigidbody2D objectRb = hit.collider.GetComponent<Rigidbody2D>();
            if (objectRb != null)
            {
                objectRb.AddForce(pushDir * pushForce * objectRb.mass, ForceMode2D.Impulse);
            }
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;

        TriggerDeathServerRpc();
    }

    [ServerRpc]
    void SetRunningServerRpc(bool running)
    {
        SetRunningClientRpc(running);
    }

    [ClientRpc]
    void SetRunningClientRpc(bool running)
    {
        animator.SetBool("isRunning", running);
    }

    [ServerRpc(RequireOwnership = false)]
    void TriggerDeathServerRpc()
    {
        TriggerDeathClientRpc();
    }

    [ClientRpc]
    void TriggerDeathClientRpc()
    {
        animator.SetTrigger("Die");
    }
}