using UnityEngine;

public class Slipstream : CarComponent
{
    [SerializeField]
    private Transform m_RaycastTransform;

    private const float m_SlipstreamLength = 10f;
    private const float m_SlipstreamBoost = 15f;

    private Car m_SlipstreamCar;

    [SerializeField]
    private LayerMask m_RaycastLayerMask;

    private void Update()
    {
        CheckSlipstream();
    }

    private void CheckSlipstream()
    {
        if (Physics.SphereCast(transform.position, 2f ,transform.forward, out var hit, m_SlipstreamLength, LayerMasks.CarLayerMask))
        {
            if (hit.collider.gameObject != gameObject)
            {
                if (hit.collider.gameObject.TryGetComponent(out Car car))
                {
                    if (m_SlipstreamCar == null)
                    {
                        m_SlipstreamCar = car;
                        AddConditionalModifier();
                    }
                }
                else
                {
                    m_SlipstreamCar = null;
                }
            }
        }
        else
        {
            m_SlipstreamCar = null;
        }
    }

    private void AddConditionalModifier()
    {
        Car.StatusManager.AddConditionalModifier(Stat.Speed,
            ModifierType.Multiplier,
        () =>
        {
            if (m_SlipstreamCar == null)
            {
                return 1f;
            }

            var distance = Vector3.Distance(transform.position, m_SlipstreamCar.transform.position);
            var proximityFactor = Mathf.Clamp01(1f - distance / m_SlipstreamLength);

            return 1f + (m_SlipstreamBoost * proximityFactor * proximityFactor);
        },
        () =>
        {
            return m_SlipstreamCar != null;
        });
    }
}
