using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : NetworkBehaviour
{
    [SerializeField] private string levelName;

    public void LoadLevel()
    {
        Debug.LogWarning("Click");
        if (!IsServer) return;
        var status = NetworkManager.Singleton.SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning("Failed to load scene");
        }
    }
}