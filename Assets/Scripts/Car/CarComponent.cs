using UnityEngine;

public abstract class CarComponent : MonoBehaviour
{
    private Car m_Car;

    protected Car Car
    {
        get => m_Car; 
        set => m_Car = value;
    }

    protected virtual void Awake()
    {
        m_Car = GetComponentInParent<Car>();
    }
}
