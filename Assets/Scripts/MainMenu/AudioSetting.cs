using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    public Slider MusicSlider;
    public Slider SfxSlider;

    public void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("musicVolume", 1f);
        SfxSlider.value = PlayerPrefs.GetFloat("sfxVolume", 1f);

        MusicSlider.onValueChanged.AddListener(SetMusicVolume);
        SfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.instance.SetMusicVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        AudioManager.instance.SetSFXVolume(volume);
    }
}