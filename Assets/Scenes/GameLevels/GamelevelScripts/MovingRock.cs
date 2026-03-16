using Unity.Netcode;
using UnityEngine;

public class MovingRock : NetworkBehaviour
{
    public float moveDistance = 4f;
    public float moveSpeed = 3f;

    private Vector3 centerPos;
    private Vector3 leftPos;
    private Vector3 rightPos;

    private int state = 0;
    private bool isActive = false;

    void Start()
    {
        centerPos = transform.position;
        leftPos = centerPos + Vector3.left * moveDistance;
        rightPos = centerPos + Vector3.right * moveDistance;
    }

    void Update()
    {
        if (!IsServer) return;
        if (!isActive) return;

        Vector3 target = centerPos;

        switch (state)
        {
            case 0: target = leftPos; break;     // center -> left
            case 1: target = centerPos; break;   // left -> center
            case 2: target = rightPos; break;    // center -> right
            case 3: target = centerPos; break;   // right -> center
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            state = (state + 1) % 4; // loop sequence
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartMovingServerRpc()
    {
        isActive = true;
    }
}