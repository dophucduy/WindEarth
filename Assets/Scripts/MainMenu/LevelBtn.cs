using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.Netcode;


public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    private int levelIndex;

    public void Setup(int index)
    {
        levelIndex = index;

        if (levelText == null)
            levelText = GetComponentInChildren<TextMeshProUGUI>();

        levelText.text = index.ToString();

        GetComponent<Button>().onClick.AddListener(LoadLevel);
    }

    public void LoadLevel()
    {
        GameData.SelectedLevel = levelIndex;

        Debug.Log("Selected Scene: " + GameData.SelectedLevel);

        //SceneManager.LoadScene("Lobby");
        if (!NetworkManager.Singleton.IsServer) return;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            var playerObj = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (playerObj != null)
                playerObj.Despawn(true);
        }
        NetworkManager.Singleton.SceneManager.LoadScene("Level" + GameData.SelectedLevel, LoadSceneMode.Single);
    }
}