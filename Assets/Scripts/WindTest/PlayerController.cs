using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class SmashMovement : NetworkBehaviour 
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private Rigidbody2D rb;
    
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
        if (!IsOwner) return; 

        moveInput = controls.Gameplay.Move.ReadValue<Vector2>();

        if (moveInput.x > 0) 
        {
            transform.localScale = new Vector3(Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
        }
        else if (moveInput.x < 0) 
        {
            transform.localScale = new Vector3(-Mathf.Abs(defaultScale.x), defaultScale.y, defaultScale.z);
        }
    }

    void FixedUpdate()
    {
        if (!IsOwner) return; 

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        
        if (isPushing)
        {
            PushObject();
        }
    }

    void Jump()
    {
        if (!IsOwner) return;

        if (isGrounded)
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
}