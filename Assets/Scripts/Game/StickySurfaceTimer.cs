using UnityEngine;

public class StickySurfaceTimer
{
    private WheelSurfaceData m_Surface;
    private float m_TimeLeft;
    private float m_Duration;

    public float TimeLeft
    {
        get => m_TimeLeft;
        set => m_TimeLeft = value;
    }

    public WheelSurfaceData Surface
    {
        get => m_Surface;
        set => m_Surface = value;
    }

    public float Duration
    {
        get => m_Duration;
        set => m_Duration = value;
    }

    public float Strength
    {
        get => TimeLeft / Duration;
    }
}
