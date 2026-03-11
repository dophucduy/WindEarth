using Unity.Netcode;
using UnityEngine;

public class MovingRock : NetworkBehaviour
{
    public float moveDistance = 4f;
    public float moveSpeed = 3f;

    private Vector3 leftPos;
    private Vector3 rightPos;

    private bool movingRight = true;
    private bool isActive = false;

    void Start()
    {
        leftPos = transform.position;
        rightPos = transform.position + Vector3.right * moveDistance;
    }

    void Update()
    {
        if (!IsServer) return;
        if (!isActive) return;

        Vector3 target = movingRight ? rightPos : leftPos;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            movingRight = !movingRight; // reverse direction
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartMovingServerRpc()
    {
        isActive = true; // start loop
    }
}