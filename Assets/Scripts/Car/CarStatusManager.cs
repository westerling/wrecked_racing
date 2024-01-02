using UnityEngine;

public class CarStatusManager : MonoBehaviour
{
    private float m_SpeedModifier = 0;
    private float m_SteerModifier = 0;

    public float SteerModifier 
    {
        get => m_SteerModifier; 
        set => m_SteerModifier = value; 
    }

    public float SpeedModifier 
    { 
        get => m_SpeedModifier; 
        set => m_SpeedModifier = value; 
    }

    public void SetSpeedModifier(float amount)
    {
        SpeedModifier = amount;
    }
}
