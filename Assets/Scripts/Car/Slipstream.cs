using UnityEngine;

public class Slipstream : CarComponent
{
    [SerializeField]
    private Transform m_RaycastTransform;

    private const float m_SlipstreamLength = 10f;
    private const float m_SlipstreamBoost = 100f;

    private Car m_SlipstreamCar;

    [SerializeField]
    private LayerMask m_RaycastLayerMask;

    private void Start()
    {
        AddConditionalModifier();
    }

    private void Update()
    {
        CheckSlipstream();
    }

    private void CheckSlipstream()
    {
        if (Physics.SphereCast(transform.position, 2f ,transform.forward, out var hit, m_SlipstreamLength, LayerMasks.SlipstreamLayerMask))
        {
            if (hit.collider.gameObject != gameObject)
            {
                if (hit.collider.gameObject.TryGetComponent(out Car car))
                {
                    m_SlipstreamCar = car;
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
        Car.StatusManager.AddConditionalModifier(Stat.Speed, () =>
        {
            if (m_SlipstreamCar == null)
            {
                return 0f;
            }
            
            var distance = Vector3.Distance(transform.position, m_SlipstreamCar.transform.position);
            var proximityFactor = Mathf.Clamp01(1f - distance / m_SlipstreamLength);

            return m_SlipstreamBoost * proximityFactor * proximityFactor;
        },
    () =>
    {
        return m_SlipstreamCar != null && Vector3.Distance(transform.position, m_SlipstreamCar.transform.position) < m_SlipstreamLength;
    });
    }
}
