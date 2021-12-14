using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer Am_AudioMixer;


    public void ChangeMaster(float vol)
    {
        Am_AudioMixer.SetFloat("MasterVol", AdaptValue(vol));

    }

    public void ChangeMusic(float vol)
    {
        Am_AudioMixer.SetFloat("MusicVol", AdaptValue(vol));
    }

    public void ChangeSounds(float vol)
    {
        Am_AudioMixer.SetFloat("SoundVol", AdaptValue(vol));
    }


    float AdaptValue(float f)
    {
        float temp = f / 10;
        return Mathf.Log10(temp) * 20;
    }
    
}
