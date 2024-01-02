using UnityEngine;

public class CarComponent : MonoBehaviour
{
    private Car m_Car;

    private void Awake()
    {
        m_Car = GetComponentInParent<Car>();
    }

    protected Car Car { get => m_Car; set => m_Car = value; }
}
