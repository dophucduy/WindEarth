using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform gridParent;
    public int totalLevels = 12;

    void Start()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            int levelIndex = i;
            GameObject btn = Instantiate(levelButtonPrefab, gridParent);
            btn.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Level " + i;

            btn.GetComponent<UnityEngine.UI.Button>()
               .onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }
}
