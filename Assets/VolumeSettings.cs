using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour //The video I used as reference for this is https://www.youtube.com/watch?v=G-JUp8AMEx0
{
    [SerializeField] private AudioMixer AudioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetsfxVolume();    //The video also added the SFX volume here. It makes sense but looks so ugly lol
        }

    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        AudioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetsfxVolume() //I wrote the SFX part even without adding anything just because
    {
        float volume = sfxSlider.value;
        AudioMixer.SetFloat("SFX", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetMusicVolume();
        SetsfxVolume();
    }
}
