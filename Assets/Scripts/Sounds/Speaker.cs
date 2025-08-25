using System;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;

    private void Start()
    {
        AddListeners();
    }

    private void OnNewSong(AudioClip audioClip)
    {
        m_AudioSource.clip = audioClip;
        m_AudioSource.volume = 80f;
        m_AudioSource.Play();
    }

    private void OnStopPlaying()
    {
        if (m_AudioSource == null)
        {
            return;
        }

        m_AudioSource.Stop();
    }

    private void AddListeners()
    {
        MusicManager.Current.PlaySong += OnNewSong;
        MusicManager.Current.StopPlaying += OnStopPlaying;
        MusicManager.Current.PauseSong += OnPause;
        MusicManager.Current.ResumeSong += OnResume;
    }

    private void OnPause()
    {
        if (m_AudioSource == null)
        {
            return;
        }

        m_AudioSource.Pause();
    }

    private void OnResume()
    {
        if (m_AudioSource == null)
        {
            return;
        }

        m_AudioSource.Play();
    }

    private void RemoveListeners()
    {
        MusicManager.Current.PlaySong -= OnNewSong;
        MusicManager.Current.StopPlaying -= OnStopPlaying;
        MusicManager.Current.PauseSong -= OnPause;
        MusicManager.Current.ResumeSong -= OnResume;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
