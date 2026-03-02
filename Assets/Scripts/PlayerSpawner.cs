using UnityEngine;
using Unity.Netcode;
using System.Collections.Generic;

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
        if (!IsServer) return;
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayerForClient;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            SpawnPlayerForClient(clientId);
        }
        
    }

    private void SpawnPlayerForClient(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject != null)
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
        //playerInstance.transform.position = chosenSpawnPoint.position;
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayerForClient;
        }
    }
}