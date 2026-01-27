using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour 
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Levels");
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
