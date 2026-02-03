using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour 
{
    public string LevelSelect;
    public void PlayGame()
    {
        SceneManager.LoadScene(LevelSelect);
    }

    public void OpenSettings()
    {
        Debug.Log("Open Settings Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
