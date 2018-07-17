using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public List<AudioSource> audioSources;
    public List<AudioClip> listOfSounds;
    public Slider musicSliderVolume;
    public Text musicTextVolume;
    public Slider soundEffectsSliderVolume;
    public Text soundEffectsTextVolume;
    public Slider generalSliderVolume;
    public Text generalTextVolume;

    void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        musicTextVolume.text = ((int)(musicSliderVolume.value * 100)).ToString() + " %";
        soundEffectsTextVolume.text = ((int)(soundEffectsSliderVolume.value * 100)).ToString() + " %";
        generalTextVolume.text = ((int)(generalSliderVolume.value * 100)).ToString() + " %";
    }

    public void Start()
    {
        PlaySound(SoundsEnum.AMBIENT,true, musicSliderVolume.value);
        GeneralVolume();
        ChangeMusicVolume();
    }

    public void GeneralVolume()
    {
        AudioListener.volume = generalSliderVolume.value;
    }

    public void ChangeMusicVolume()
    {
        audioSources[0].volume = musicSliderVolume.value;
    }

    public void ChangeSoundEffectsVolume()
    {
        for (int i = 1; i < audioSources.Count; i++)
        {
            audioSources[i].volume = soundEffectsSliderVolume.value;
        }
    }

    public void PlaySound(SoundsEnum clip, bool loop = false, float volume = 1)
    {
        //No tengo audiosources? Creo uno y reproduzco el sonido
        if (audioSources.Count == 0)
        {
            var toplay = gameObject.AddComponent<AudioSource>();
            toplay.clip = listOfSounds[(int)clip];
            toplay.loop = loop;
            toplay.volume = soundEffectsSliderVolume.value;
            audioSources.Add(toplay);
            toplay.Play();
            // Debug.Log("guarde la cancion");
        }
        // SI tengo audiosources? busco si el sonido esta en alguno y reproduzco, sino creo otro.
        else
        {
            //Hay algun audiosource sin usarse? lo uso.
            foreach (var item in audioSources)
            {
                if (!item.isPlaying)
                {
                    item.clip = listOfSounds[(int)clip];
                    item.loop = loop;
                    item.Play();
                    return;
                }
            }
            //Estan todos usandose y tengo el sonido guardado? lo vuelvo a reproducir
            foreach (var temp in audioSources)
            {
                if (temp.clip.name == listOfSounds[(int)clip].name)
                {
                    temp.Play();
                    temp.loop = loop;
                    // Debug.Log("la encontre, y vuelvo a reproducirla");
                    return;
                }
            }
            //No queda otra que agregar otro audiosource
            var toplay2 = gameObject.AddComponent<AudioSource>();
            toplay2.clip = listOfSounds[(int)clip];
            toplay2.loop = loop;
            toplay2.volume = soundEffectsSliderVolume.value;
            toplay2.Play();
            audioSources.Add(toplay2);
        }
    }
}
