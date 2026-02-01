using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    public string levelSceneName;

    public void LoadLevel()
    {
        SceneManager.LoadScene(levelSceneName);
    }
}