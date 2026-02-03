using UnityEngine;

public class LevelFinishManager : MonoBehaviour
{
    public static LevelFinishManager Instance;

    public FlagTriggerSciprt[] flags;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void CheckCompletion()
    {
        foreach (FlagTriggerSciprt flag in flags)
        {
            if (!flag.IsActivated())
            {
                return; // Not finished yet
            }
        }

        LevelCompleted();
    }

    void LevelCompleted()
    {
        Debug.Log("LEVEL COMPLETED!");
        // TODO: load next level, show UI, etc.
    }
}
