using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Current;

    [SerializeField]
    private AudioSource m_AudioSource;

    [Header("Race Sounds")]
    [SerializeField]
    private AudioClip m_CountDown;

    [Header("Music")]
    [SerializeField]
    private AudioClip[] m_Music;

    private void Awake()
    {
        Current = this;
    }

    public void PlaySoundOnce(AudioClip audioClip)
    {
        //m_AudioSource.Play(audioClip);
    }
}
