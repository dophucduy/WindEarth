using UnityEngine;
using Unity.Netcode;

public class PlayerInputController : NetworkBehaviour
{
    public float moveSpeed = 5f;

    PlayerNetworkController networkController;

    void Awake()
    {
        networkController = GetComponent<PlayerNetworkController>();
    }

    void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
        HandleAbilityInput();
        HandleInteractInput();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector2.right * h * moveSpeed * Time.deltaTime);
    }

    void HandleAbilityInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            networkController.AbilityServerRpc();
        }
    }
    void HandleInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            networkController.InteractServerRpc();
        }
    }
}
