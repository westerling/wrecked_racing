using UnityEngine;

public class Suspension : CarComponent
{
    [SerializeField]
    private Transform m_TireTransform;

    private Vector3 m_CurrentWheelPosition;

    private float m_WheelRadius;

    private void Start()
    {
        if (m_TireTransform.gameObject.TryGetComponent(out Wheel wheel))
        {
            m_WheelRadius = wheel.CalculateWheelRadius();
        }
    }

    private void Update()
    {
        m_CurrentWheelPosition = m_TireTransform.position;
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var ray, Car.Stats.SuspensionRestDistance + m_WheelRadius))
        {
            var springDirection = transform.up;
            var tireWorldVelocity = Car.Rigidbody.GetPointVelocity(m_TireTransform.position);
            var offset = Car.Stats.SuspensionRestDistance - ray.distance;
            var velocity = Vector3.Dot(springDirection, tireWorldVelocity);
            var force = (offset * Car.Stats.SpringStrength) - (velocity * Car.Stats.SpringDamper);

            m_TireTransform.position = ray.point + (transform.up * m_WheelRadius);

            Car.Rigidbody.AddForceAtPosition(springDirection * force, ray.point);
        }
    }
}
