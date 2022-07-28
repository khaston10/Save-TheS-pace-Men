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

    public void PlaySound(AudioClip clip)
    {
        _fxSource.PlayOneShot(clip);
    }

    public void ChangeMasterVol(float volume)
    {
        AudioListener.volume = volume;
    }
}
