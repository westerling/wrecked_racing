using UnityEngine;

public class SoundFx : MonoBehaviour
{
    [SerializeField]
    private SoundFxType m_SoundFxType;

    [SerializeField]
    private AudioClip m_AudioClip;

    public SoundFxType SoundFxType
    {
        get => m_SoundFxType;
        set => m_SoundFxType = value;
    }

    public AudioClip AudioClip
    {
        get => m_AudioClip;
        set => m_AudioClip = value;
    }
}
