using UnityEngine;

public class RaceSettings
{
    private GameObject m_Car;
    private bool m_AirstrikeOn;
    private int m_SceneIndex;
    
    private int m_NumberOfBots;


    public GameObject Car 
    { 
        get => m_Car; 
        set => m_Car = value; 
    }
    
    public int SceneIndex
    {
        get => m_SceneIndex; 
        set => m_SceneIndex = value;
    }
    
    public int NumberOfBots 
    { 
        get => m_NumberOfBots; 
        set => m_NumberOfBots = value; 
    }

    public bool AirstrikeOn
    {
        get => m_AirstrikeOn;
        set => m_AirstrikeOn = value;
    }
}
