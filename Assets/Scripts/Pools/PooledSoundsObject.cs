using System;
using UnityEngine;

[Serializable]
public class PooledSoundsObject
{
    [SerializeField]
    private AudioClip m_ObjectToPool;

    [SerializeField]
    private SoundFxType m_SoundFxType;

    [SerializeField]
    private int m_Amount;

    public AudioClip ObjectToPool
    {
        get => m_ObjectToPool;
    }

    public SoundFxType SoundFxType
    {
        get => m_SoundFxType;
    }

    public int Amount
    {
        get => m_Amount;
    }
}
