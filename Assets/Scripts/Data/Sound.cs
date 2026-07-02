using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Objects/Sound")]
public class Sound : ScriptableObject
{
    [SerializeField]
    private AudioClip m_AudioClip;

    [SerializeField]
    [Range(0f, 1f)]
    private float m_Volume = 1f;

    [SerializeField]
    [Range(0f, 0.5f)]
    private float m_VolumeRandomness = 0f;

    [SerializeField]
    [Range(0.5f, 2f)]
    private float m_Pitch = 1f;

    [SerializeField]
    [Range(0f, 0.5f)]
    private float m_PitchRandomness = 0f;

    public AudioClip AudioClip
    {
        get => m_AudioClip;
    }

    public float Volume
    {
        get => m_Volume;
    }

    public float Pitch
    {
        get => m_Pitch;
    }

    public float PitchRandomness
    {
        get => m_PitchRandomness;
    }

    public float VolumeRandomness
    {
        get => m_VolumeRandomness;
    }
}
