using UnityEngine;

public class CarRaceStats
{
    private int m_Points;
    private bool m_IsActive;

    private GameObject m_Car;

    public bool IsActive 
    {
        get => m_IsActive;
        set => m_IsActive = value;
    }

    public int Points 
    { 
        get => m_Points; 
        set => m_Points = value; 
    }

    public GameObject Car 
    {
        get => m_Car;
        set => m_Car = value; 
    }
}
