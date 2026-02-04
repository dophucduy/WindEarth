using Unity.Netcode;
using UnityEngine;

public class PlayerNetworkController : NetworkBehaviour
{
    public NetworkVariable<PlayerRole> Role =
        new NetworkVariable<PlayerRole>(
            PlayerRole.Wind,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            AssignRole();
        }

        Role.OnValueChanged += OnRoleChanged;
        Debug.Log(
       $"[{(IsServer ? "SERVER" : "CLIENT")}] " +
       $"Spawned | Owner={OwnerClientId} | Local={NetworkManager.Singleton.LocalClientId} | IsOwner={IsOwner}"
   );
    }

    void AssignRole()
    {
        // clientId = 0 → Wind
        // clientId = 1 → Earth
        Role.Value = OwnerClientId == 0 ? PlayerRole.Wind : PlayerRole.Earth;

        Debug.Log($"[SERVER] Assigned role {Role.Value} to client {OwnerClientId}");
    }

    void OnRoleChanged(PlayerRole oldRole, PlayerRole newRole)
    {
        Debug.Log($"Role changed: {oldRole} → {newRole} (Owner={OwnerClientId})");

        ApplyRoleVisual(newRole);
    }

    void ApplyRoleVisual(PlayerRole role)
    {
        // tạm thời chỉ log, bước sau mới enable ability
        if (role == PlayerRole.Wind)
            Debug.Log("This is WIND player");
        else
            Debug.Log("This is EARTH player");
    }

    [ServerRpc]
    public void AbilityServerRpc()
    {
        Debug.Log($"[SERVER] Ability request from client {OwnerClientId}");

        if (Role.Value == PlayerRole.Wind)
            DoWindAbility();
        else if (Role.Value == PlayerRole.Earth)
            DoEarthAbility();
    }

    void DoWindAbility()
    {
        Debug.Log("SERVER: Wind ability");
        // TODO:  Wind 
    }

    void DoEarthAbility()
    {
        Debug.Log("SERVER: Earth ability");
        // TODO:  Earth 
    }

    [ServerRpc]
    public void InteractServerRpc()
    {
        Debug.Log($"[SERVER] Client {OwnerClientId} requested INTERACT");

        // TODO: gameplay team xử lý tương tác ở đây
    }


}
