using Unity.Netcode;
using UnityEngine;

public class SceneMusic : NetworkBehaviour
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