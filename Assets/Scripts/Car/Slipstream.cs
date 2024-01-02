using System.Collections.Generic;
using UnityEngine;

public class Slipstream : CarComponent
{
    private Car m_OwnCar;

    private float m_SlipstreamReach;

    private List<Car> m_SlipstreamList = new List<Car>();

    private void Start()
    {
        m_OwnCar = GetComponentInParent<Car>();

        if (m_OwnCar = null)
        {
            Destroy(this);
        }

        SetSlipstreamReach();
    }

    private void Update()
    {
        foreach (var car in m_SlipstreamList)
        {
            var distanceToObject = Vector3.Distance(transform.position, car.transform.position);
            var difference = m_SlipstreamReach - distanceToObject;
            var slipstreamEffect = car.Stats.SlipstreamEffect - (difference / m_SlipstreamReach);

            var speed = Mathf.Abs(Vector3.Dot(Car.transform.forward, Car.Rigidbody.velocity));
            var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(speed) / Car.Stats.TopSpeed);

            var speedModifier = slipstreamEffect * normalizedSpeed;

            car.StatusManager.SetSpeedModifier(speedModifier);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponentInParent<Car>();

        if (car == null)
        {
            return;
        }

        if (car == m_OwnCar || m_SlipstreamList.Contains(car))
        {
            return;
        }

        m_SlipstreamList.Add(car);
    }

    private void OnTriggerExit(Collider other)
    {
        var car = other.GetComponentInParent<Car>();

        if (car == null)
        {
            return;
        }

        if (car == m_OwnCar || !m_SlipstreamList.Contains(car))
        {
            return;
        }

        car.StatusManager.SetSpeedModifier(0);
        m_SlipstreamList.Remove(car);
    }

    private void SetSlipstreamReach()
    {
        if (TryGetComponent(out BoxCollider collider))
        {
            m_SlipstreamReach = collider.size.z;
        }
    }
}
