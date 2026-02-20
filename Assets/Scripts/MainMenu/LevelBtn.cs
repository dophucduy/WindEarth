using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

        SceneManager.LoadScene("Lobby");
    }
}