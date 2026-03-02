using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishManager : MonoBehaviour
{
    public static LevelFinishManager Instance;

    [Header("Flags (Win Condition)")]
    public FlagTriggerSciprt[] flags;

    [Header("UI")]
    public GameObject winPanel;
    public GameObject gameOverPanel;

    private bool gameEnded = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // ================= WIN CHECK =================
    public void CheckCompletion()
    {
        if (gameEnded) return;

        foreach (FlagTriggerSciprt flag in flags)
        {
            if (!flag.IsActivated())
                return;
        }

        LevelCompleted();
    }

    void LevelCompleted()
    {
        gameEnded = true;
        Debug.Log("LEVEL COMPLETED!");

        if (winPanel != null)
            winPanel.SetActive(true);

        Time.timeScale = 0f; // Pause game
    }

    // ================= GAME OVER =================
    public void GameOver()
    {
        if (gameEnded) return;

        gameEnded = true;
        Debug.Log("GAME OVER!");

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    // ================= BUTTONS =================
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
