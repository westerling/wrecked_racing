using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Current;

    public event Action<AudioClip> PlaySong;
    public event Action StopPlaying;
    public event Action PauseSong;
    public event Action ResumeSong;

    private List<AudioClip> m_MusicQueue = new();

    private int m_CurrentIndex = 0;
    private bool m_Repeat = false;
    private bool m_IsPlaying = false;
    private bool m_RandomOrder = false;

    private void Awake()
    {
        Current = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Play()
    {
        if (m_MusicQueue.Count == 0)
        {
            return;
        }

        if (m_RandomOrder)
        {
            ShuffleQueue();
        }

        m_CurrentIndex = 0;
        m_IsPlaying = true;
        PlayCurrent();
    }

    public void ResumeMusic()
    {
        ResumeSong?.Invoke();
    }

    public void AddToQueue(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            m_MusicQueue.Add(audioClip);
        }
    }

    public void AddToQueue(AudioClip[] audioClips, bool randomOrder)
    {
        ClearQueue();

        m_MusicQueue.AddRange(audioClips);
        m_RandomOrder = randomOrder;

        if (m_RandomOrder)
        {
            ShuffleQueue();
        }
    }

    public void PauseMusic()
    {
        m_IsPlaying = false;
        PauseSong?.Invoke();
    }

    public void StopMusic()
    {
        m_IsPlaying = false;
        StopPlaying?.Invoke();
    }

    private void PlayNext()
    {
        if (!m_IsPlaying)
        {
            return;
        }

        m_CurrentIndex++;

        if (m_CurrentIndex >= m_MusicQueue.Count)
        {
            if (m_Repeat)
            {
                m_CurrentIndex = 0;
            }
            else
            {
                StopMusic();
                return;
            }
        }

        PlayCurrent();
    }

    private void PlayCurrent()
    {
        if (m_CurrentIndex >= 0 && m_CurrentIndex < m_MusicQueue.Count)
        {
            var audioClip = m_MusicQueue[m_CurrentIndex];

            PlaySong?.Invoke(audioClip);
            StartCoroutine(WaitForSongToEnd(audioClip.length));
        }
    }

    private void ClearQueue()
    {
        m_MusicQueue.Clear();
        m_CurrentIndex = 0;
    }

    private void ShuffleQueue()
    {
        var random = new System.Random();
        var i = m_MusicQueue.Count;
        
        while (i > 1)
        {
            i--;
            var j = random.Next(i + 1);
            (m_MusicQueue[i], m_MusicQueue[j]) = (m_MusicQueue[j], m_MusicQueue[i]);
        }
    }

    private IEnumerator WaitForSongToEnd(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        PlayNext();
    }
}
