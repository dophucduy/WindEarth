using Unity.Netcode;
using UnityEngine;

public class Goal : NetworkBehaviour
{
    [SerializeField] private int requiredPlayerId;
    [SerializeField] private Animator animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsServer) return;

        NetworkObject netObj = other.GetComponent<NetworkObject>();
        if (netObj == null || !netObj.IsPlayerObject) return;

        if (netObj.OwnerClientId == (ulong)requiredPlayerId)
        {
            animator.SetTrigger("Raise"); // play flag animation

            VictoryManager.Singleton.PlayerReachedGoal(requiredPlayerId);
        }
    }
}