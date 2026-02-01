using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

     [Header("Movement Settings")]
    private PlayerControls controls; 
    private Vector2 moveInput;
    private bool isGrounded;

    [Header("Push Settings")]
    public float pushRange = 1.5f;
    public float pushForce = 10f; 
    public LayerMask pushLayer;

    private bool isPushing;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Jump.performed += context => Jump();
        controls.Gameplay.Push.started += context => isPushing = true;
        controls.Gameplay.Push.canceled += context => isPushing = false;
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        moveInput = controls.Gameplay.Move.ReadValue<Vector2>();
        if (moveInput.x > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isPushing)
        {
            PushObject();
        }
    }

    void Jump()
    {
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