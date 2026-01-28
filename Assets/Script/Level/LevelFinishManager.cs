using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinishManager : MonoBehaviour
{
    public static LevelFinishManager Instance;

    public FlagTrigger[] flags;
    private bool completed = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckCompletion()
    {
        if (completed) return;

        foreach (var flag in flags)
        {
            if (!flag.IsActivated())
                return;
        }

        completed = true;
        Debug.Log("LEVEL COMPLETE!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
