using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class OptionsMenu : MonoBehaviour
{
    public Slider mSlider;
    public Toggle mToggle;
    public TMPro.TMP_Dropdown mDropdown;

    public AudioMixer audioMixer;
    public GameObject MenuOfOrigin;
    public TMPro.TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    void Awake()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }
        resolutionDropdown.AddOptions(options);
        mSlider.value = DATASAVER.MVolume;
        mToggle.isOn = DATASAVER.IFullscreen;
        mDropdown.value = DATASAVER.SResolution;
    }
    public void SetResolution (int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        DATASAVER.SResolution = ResolutionIndex;
    }
    public void OptionsMenuOpen ()
    {
        gameObject.SetActive(true);
        MenuOfOrigin.SetActive(false);
    }
    public void OptionsMenuClose ()
    {
        gameObject.SetActive(false);
        MenuOfOrigin.SetActive(true);
    }
    public void SetVolume (float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        DATASAVER.MVolume = volume;
    }
    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        DATASAVER.IFullscreen = isFullScreen;
    }
}
