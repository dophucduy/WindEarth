using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : NetworkBehaviour
{
    //[SerializeField] private string levelName = "Level1";

    public void LoadLevel()
    {
        if (!IsServer) return;
        //var status = NetworkManager.Singleton.SceneManager.LoadScene("Level" + GameData.SelectedLevel, LoadSceneMode.Single);
        var status = NetworkManager.Singleton.SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        if (status != SceneEventProgressStatus.Started)
        {
            Debug.LogWarning("Failed to load scene");
        }
    }
}