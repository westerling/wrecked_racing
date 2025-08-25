using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    public static SoundMixerManager Current;

    [SerializeField]
    private AudioMixer m_AudioMixer;

    private void Awake()
    {
        Current = this;
    }

    public void SetMasterVolume(float level)
    {
        m_AudioMixer.SetFloat("MasterVolume", level);
    }

    public void SetMusicVolume(float level)
    {
        m_AudioMixer.SetFloat("MusicVolume", level);
    }

    public void SetSoundFxVolume(float level)
    {
        m_AudioMixer.SetFloat("SoundFxVolume", level);
    }
}
