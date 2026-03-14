using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    public AudioClip sceneMusic;

    void Start()
    {
        if (AudioManager.instance != null && sceneMusic != null)
        {
            AudioManager.instance.ChangeMusic(sceneMusic);
        }
    }
}
