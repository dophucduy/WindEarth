using Unity.Netcode;
using UnityEngine;

public class CustomPlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject windPrefab;
    [SerializeField] private GameObject earthPrefab;

    private void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject prefabToSpawn;

        // Host luôn có clientId = 0
        if (clientId == 0)
        {
            prefabToSpawn = windPrefab;
        }
        else
        {
            prefabToSpawn = earthPrefab;
        }

        GameObject playerInstance = Instantiate(prefabToSpawn);

        playerInstance.GetComponent<NetworkObject>()
            .SpawnAsPlayerObject(clientId, true);
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
            NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
    }
}
