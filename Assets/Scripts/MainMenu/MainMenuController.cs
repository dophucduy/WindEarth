using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour 
{
    public string LevelSelect;
    public void PlayGame()
    {
        //SceneManager.LoadScene(LevelSelect);
        SceneManager.LoadScene("Lobby");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Setting", LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}
