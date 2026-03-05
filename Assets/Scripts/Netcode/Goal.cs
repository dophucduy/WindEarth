using Unity.Netcode;
using UnityEngine;

public class Goal : NetworkBehaviour
{
    [SerializeField] private int requiredPlayerId;
    // 0 = Player1/wind, 1 = Player2/earth

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        NetworkObject netObj = other.GetComponent<NetworkObject>();
        if (netObj == null || !netObj.IsPlayerObject) return;

        if (netObj.OwnerClientId == (ulong)requiredPlayerId)
        {
            VictoryManager.Singleton.PlayerReachedGoal(requiredPlayerId);
        }
    }
}