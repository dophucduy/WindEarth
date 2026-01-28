using UnityEngine;
using TMPro;
using System.Collections;

public class TutorialTextFade : MonoBehaviour
{
    public float delayBeforeFade = 5f;
    public float fadeDuration = 1.5f;

    private TextMeshPro text;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();

        if (text == null)
            Debug.LogError("TutorialTextFade: No TextMeshPro found!", this);
    }

    void OnEnable()
    {
        if (text != null)
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(delayBeforeFade);

        Color c = text.color;
        float startAlpha = c.a;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            text.color = c;
            yield return null;
        }

        c.a = 0f;
        text.color = c;
        gameObject.SetActive(false);
    }
}
