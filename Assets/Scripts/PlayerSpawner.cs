using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject player1Prefab;
    [SerializeField] private GameObject player2Prefab;

    [Header("Spawn Points")]
    [SerializeField] private Transform spawnPointP1;
    [SerializeField] private Transform spawnPointP2;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayerForClient;
             if (!NetworkManager.Singleton.LocalClient.PlayerObject)
            {
                 SpawnPlayerForClient(NetworkManager.Singleton.LocalClientId);
            }
        }
    }

    private void SpawnPlayerForClient(ulong clientId)
    {if (NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null)
        {
            return;
        }

        GameObject chosenPrefab;
        Transform chosenSpawnPoint;

        if (clientId == 0)
        {
            chosenPrefab = player1Prefab;
            chosenSpawnPoint = spawnPointP1;
        }
        else
        {
            chosenPrefab = player2Prefab;
            chosenSpawnPoint = spawnPointP2;
        }

        GameObject playerInstance = Instantiate(chosenPrefab, chosenSpawnPoint.position, Quaternion.identity);
        
        playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayerForClient;
        }
    }
}