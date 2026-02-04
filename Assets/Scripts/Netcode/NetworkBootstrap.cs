using Unity.Netcode;
using UnityEngine;

public class NetworkBootstrap : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
    }

    void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client CONNECTED: {clientId}");
        Debug.Log($"Total clients: {NetworkManager.Singleton.ConnectedClients.Count}");
    }

    void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client DISCONNECTED: {clientId}");
    }

    public void Host()
    {
        Debug.Log("Host clicked");
        NetworkManager.Singleton.StartHost();
    }

    public void Join()
    {
        Debug.Log("Join clicked");
        NetworkManager.Singleton.StartClient();
    }
}
