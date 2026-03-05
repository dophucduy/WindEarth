using UnityEngine;
using Unity.Netcode;

public class FallingPlatform : NetworkBehaviour
{
    public float fallSpeed = 2f;
    public float riseSpeed = 2f;
    public float riseDelay = 2f;

    private int playersOnPlatform = 0;
    private float leaveTimer = 0f;
    private Vector3 startPosition;

    private Rigidbody2D rb;

    public override void OnNetworkSpawn()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!IsServer) return;

        if (playersOnPlatform > 0)
        {
            leaveTimer = 0f;
            rb.MovePosition(rb.position + Vector2.down * fallSpeed * Time.fixedDeltaTime);
        }
        else
        {
            leaveTimer += Time.fixedDeltaTime;

            if (leaveTimer >= riseDelay)
            {
                Vector2 newPos = Vector2.MoveTowards(
                    rb.position,
                    startPosition,
                    riseSpeed * Time.fixedDeltaTime
                );

                rb.MovePosition(newPos);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.TryGetComponent<NetworkObject>(out var netObj)
            && netObj.IsPlayerObject)
        {
            playersOnPlatform++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!IsServer) return;

        if (collision.gameObject.TryGetComponent<NetworkObject>(out var netObj)
            && netObj.IsPlayerObject)
        {
            playersOnPlatform--;
        }
    }
}