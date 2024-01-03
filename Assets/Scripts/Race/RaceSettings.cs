using UnityEngine;

public class RaceSettings
{
    private GameObject m_Car;

    private int m_SceneIndex;
    
    private float m_GameTime;

    private bool m_UseGameTimer;

    private bool m_AirStrike;

    public GameObject Car 
    { 
        get => m_Car; 
        set => m_Car = value; 
    }

    public float GameTime
    {
        get => m_GameTime; 
        set => m_GameTime = value;
    }
    
    public bool UseGameTimer 
    {
        get => m_UseGameTimer; 
        set => m_UseGameTimer = value;
    }

    public bool AirStrike 
    { 
        get => m_AirStrike; 
        set => m_AirStrike = value;
    }
    
    public int SceneIndex 
    {
        get => m_SceneIndex; 
        set => m_SceneIndex = value;
    }
}
