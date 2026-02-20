using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform[] levelSlots;
    public int totalLevels = 5;

    void Start()
    {
        for (int i = 0; i < totalLevels && i < levelSlots.Length; i++)
        {
            GameObject btn = Instantiate(levelButtonPrefab, levelSlots[i]);

            btn.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            btn.GetComponent<LevelButton>().Setup(i + 1);
        }
    }
}
