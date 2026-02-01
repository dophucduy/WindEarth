using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        Debug.Log(
            $"Player spawned | Owner={OwnerClientId} | IsServer={IsServer} | IsOwner={IsOwner}"
        );
    }
}
