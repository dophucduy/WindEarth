using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SmashMovement : NetworkBehaviour 
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private Rigidbody2D rb;
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

    private void Awake()
    {

        animator = GetComponent<Animator>();

        controls = new PlayerControls();
        
        controls.Gameplay.Jump.performed += context => Jump();
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
    }

    public override void OnNetworkDespawn()
    {
        if (controls != null)
        {
            controls.Disable();
        }
    }

    void Update()
    {
        //if (!IsOwner || isDead) return; 

        //moveInput = controls.Gameplay.Move.ReadValue<Vector2>();

        //if (moveInput.x > 0) 
        //{
        //    transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
        //}
        //else if (moveInput.x < 0) 
        //{
        //    transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
        //}

        //// Animation
        //animator.SetBool("isRunning", Mathf.Abs(moveInput.x) > 0.1f);
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
        //if (!IsOwner || isDead) return; 

        //rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        //animator.SetBool("isJumping", !isGrounded);

        //animator.SetBool("isFalling", rb.linearVelocity.y < -0.1f && !isGrounded);

        //if (isPushing)
        //{
        //    PushObject();
        //}
        if (IsOwner)
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
    }

    void Jump()
    {
        if (!IsOwner) return;

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
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