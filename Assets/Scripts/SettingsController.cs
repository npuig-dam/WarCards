using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle fullscreenToggle;

    void Start()
    {
      
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);

     
        AudioListener.volume = savedVolume;

    
        if (volumeSlider != null)
        {
          
            volumeSlider.onValueChanged.RemoveListener(SetVolume);

            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value = savedVolume;


            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.isOn = Screen.fullScreen;
        }
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
 
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
      
        PlayerPrefs.SetInt("IsFullscreen", isFullscreen ? 1 : 0);
    }
}