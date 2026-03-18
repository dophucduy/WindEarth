using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadVolume(); 
        }
        else { Destroy(gameObject); }
    }

    public void ChangeMusic(AudioClip newClip)
    {
        if (musicSource.clip == newClip) return;

        musicSource.clip = newClip;
        musicSource.Play();
    }

    public void SetMusicVolume(float volume)
    {
        // Chuyển đổi giá trị slider (0-1) sang Decibel (-80 đến 20)
        audioMixer.SetFloat("musicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    void LoadVolume()
    {
        float musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);

        // Phải đặt giá trị sau khi Mixer đã được nạp hoàn toàn
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }
}