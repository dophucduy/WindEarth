using Unity.Netcode;
using UnityEngine;

public class LeverTrigger : NetworkBehaviour
{
    public MovingRock rock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsClient) return;

        if (collision.attachedRigidbody != null)
        {
            rock.StartMovingServerRpc();
        }
    }
}
