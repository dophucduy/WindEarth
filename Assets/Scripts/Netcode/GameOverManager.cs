using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverManager : NetworkBehaviour
{
    public static GameOverManager Singleton { get; private set; }

    [SerializeField] private GameObject gameOverUI;    
    [SerializeField] private float pauseDuration = 3f;

    private void Awake()
    {
        Singleton = this;
        gameOverUI.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    public void TriggerGameOverServerRpc()
    {
        TriggerGameOverClientRpc();
    }

    [ClientRpc]
    private void TriggerGameOverClientRpc()
    {
        StartCoroutine(GameOverSequence());
    }

    private IEnumerator GameOverSequence()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(pauseDuration);

        Time.timeScale = 1f;
        gameOverUI.SetActive(false);

        if (IsServer)
        {
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                var playerObj = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
                if (playerObj != null)
                    playerObj.Despawn(true);
            }

            NetworkManager.Singleton.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                UnityEngine.SceneManagement.LoadSceneMode.Single
            );
        }
    }
}