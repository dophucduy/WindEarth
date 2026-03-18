using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public AudioClip bgm;
    public GameObject audioManagerPrefab; 

    void Start()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManagerPrefab);
        }

        if (bgm != null)
        {
            AudioManager.instance.ChangeMusic(bgm);
        }
    }
}