using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    float volume;
    string sVolume;
    public Slider volumeSlider;


    private void Awake()
    {
        //if Player prefs does not have a volume set, initialize to default value and save it 
        if (!PlayerPrefs.HasKey(sVolume))
        {

            volume = 0.5f;
            PlayerPrefs.SetFloat(sVolume, volume);
            Debug.Log("Stored default volume = " + volume);
        }
        //else update audio listener to match playerPrefs
        else    
        { 
            volume = PlayerPrefs.GetFloat(sVolume);
            AudioListener.volume = volume;
            volumeSlider.value = volume;
            Debug.Log("PlayerPref volume set = " + volume);
        }

    }

    
    //Update volume to match slider changes
    public void OnSliderValueChanged()
    {
        Debug.Log("Slider changed  to = " +  volumeSlider.value);
        volume = volumeSlider.value;
        AudioListener.volume = volume;
        Debug.Log("volume = " + volume);
    }


    public void OnDisable()
    {
        PlayerPrefs.SetFloat(sVolume, volume);
        PlayerPrefs.Save();
    }
}
