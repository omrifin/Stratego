using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SC_SoundHandler : MonoBehaviour {

    public Slider moneySlider;
    public Slider mainMusicSoundSlider;
    public Slider sFXSoundSlider;
    public AudioSource myMusic ;
    public AudioSource clickSound;

    public void MainMusicSoundSlider()
    {
        float SliderValue = mainMusicSoundSlider.value / 10;
        Debug.Log(mainMusicSoundSlider.value);
        myMusic.volume = SliderValue;
    }

    public void PlayClickSound()

    {
        clickSound.Play();
    }

    public void SFXSoundSlider()
    {
        float SliderValue = sFXSoundSlider.value / 10;
        Debug.Log(sFXSoundSlider.value);
        clickSound.volume = SliderValue;
    }

}
