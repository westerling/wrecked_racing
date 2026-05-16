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
        Car = GetComponentInParent<Car>();

        if (Car == null)
        {
            Debug.LogError("Could not find Car component in parent.");
        }
    }
}
