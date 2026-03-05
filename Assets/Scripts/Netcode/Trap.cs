using Unity.Netcode;
using UnityEngine;

public class Trap : NetworkBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        NetworkObject netObj = other.GetComponent<NetworkObject>();
        if (netObj != null && netObj.IsPlayerObject)
        {
            GameOverManager.Singleton.TriggerGameOverServerRpc();
        }
    }
}