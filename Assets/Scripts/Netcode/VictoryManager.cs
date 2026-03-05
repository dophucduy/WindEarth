using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class VictoryManager : NetworkBehaviour
{
    public static VictoryManager Singleton { get; private set; }

    [SerializeField] private GameObject levelCompleteUI;
    [SerializeField] private float pauseDuration = 3f;

    private bool player1Reached = false;
    private bool player2Reached = false;

    private void Awake()
    {
        Singleton = this;
        levelCompleteUI.SetActive(false);
    }

    public void PlayerReachedGoal(int playerId)
    {
        if (!IsServer) return;

        if (playerId == 0) player1Reached = true;
        if (playerId == 1) player2Reached = true;

        if (player1Reached && player2Reached)
        {
            LevelCompleteClientRpc();
        }
        else
        {
            PlayerWaitingClientRpc(playerId);
        }
    }

    [ClientRpc]
    private void PlayerWaitingClientRpc(int playerId)
    {
        Debug.Log($"Player {playerId + 1} reached the goal! Waiting for the other...");
    }

    [ClientRpc]
    private void LevelCompleteClientRpc()
    {
        StartCoroutine(LevelCompleteSequence());
    }

    private IEnumerator LevelCompleteSequence()
    {
        levelCompleteUI.SetActive(true);
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(pauseDuration);

        Time.timeScale = 1f;
        levelCompleteUI.SetActive(false);

        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(
                "LevelSelect",
                UnityEngine.SceneManagement.LoadSceneMode.Single
            );
        }
    }
}