using System;
using UnityEngine;

[Serializable]
public class CarPartStats
{
    [Range(0.01f, 1f)]
    [SerializeField]
    private float m_Threshold;

    [Range(1f, 100f)]
    [SerializeField]
    private float m_Mass;

    [SerializeField]
    private CarPartType m_CarPartType;

    public float Threshold
    {
        get => m_Threshold;
        set => m_Threshold = value;
    }
    public float Mass
    {
        get => m_Mass;
        set => m_Mass = value;
    }

    public CarPartType CarPartType
    {
        get => m_CarPartType;
    }
}
