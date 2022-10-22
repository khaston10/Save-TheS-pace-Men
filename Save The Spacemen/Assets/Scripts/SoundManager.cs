using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _musicSource, _fxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public float GetMusicVol()
    {
        return _musicSource.volume;
    }

    public float GetSFXVol()
    {
        return _fxSource.volume;
    }

    public void PlaySound(AudioClip clip)
    {
        _fxSource.PlayOneShot(clip);
    }

    public void ChangeMasterVol(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ChangeMusicVol(float vol)
    {
        if (vol < 0) vol = 0;
        else if (vol > 1) vol = 1;
        _musicSource.volume = vol;
    }

    public void ChangeSFXVol(float vol)
    {
        if (vol < 0) vol = 0;
        else if (vol > 1) vol = 1;
        _fxSource.volume = vol;
    }
}
