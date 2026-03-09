using GLTFast.Schema;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1);
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1);

        musicSource.volume = musicSlider.value;
        sfxSource.volume = sfxSlider.value;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
