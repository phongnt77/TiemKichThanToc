using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider volumeBackroundSlider;

    private float previousBackgroundVolume;

    private float previousBackgroundSlider;

    private void Start()
    {
        audioMixer.GetFloat("BackgroundVolume", out float previousBackgroundVolume);
        previousBackgroundSlider = volumeBackroundSlider.value;
    }

    public void HandleMuteButton()
    {
        audioMixer.SetFloat("BackgroundVolume", -80f);
    }

    public void HandleSliderValueChange()
    {
        audioMixer.SetFloat("BackgroundVolume", volumeBackroundSlider.value);
    }

    public void HandleSaveButton()
    {
        audioMixer.GetFloat("BackgroundVolume", out float previousBackgroundVolume);
        previousBackgroundSlider = volumeBackroundSlider.value;
    }
    public void HandleBackButton()
    {
        audioMixer.SetFloat("BackgroundVolume", previousBackgroundVolume);
        volumeBackroundSlider.value = previousBackgroundSlider;
    }
}
