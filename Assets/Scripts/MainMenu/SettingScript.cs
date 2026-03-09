using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingScript : MonoBehaviour
{
    public void OpenSettings()
    {
        SceneManager.LoadScene("Setting");
    }

}
