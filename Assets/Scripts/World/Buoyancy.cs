using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_RigidBody;

    private readonly float m_Speed = 1;
    private readonly float m_Frequency = 2;
    private readonly float m_WaterLevel = -5;

    private readonly float m_Buoyancy = 40;
    private readonly float m_MaxBuoyancy = 90;

    private readonly float m_OriginalDamping;
    private readonly float m_OriginalAngularDamping;
    private readonly float m_WaterDamping = 3;
    private readonly float m_WaterAngularDamping = 2;

    private bool m_IsUnderwater;

    public void SetBuoyancy(bool isUnderWater)
    {
        m_IsUnderwater = isUnderWater;
        m_RigidBody.linearDamping = isUnderWater ? m_WaterDamping : m_OriginalDamping;
        m_RigidBody.angularDamping = isUnderWater ? m_WaterAngularDamping : m_OriginalAngularDamping;
    }

    private void Start()
    {
        SetBuoyancy(false);
    }

    private void FixedUpdate()
    {
        if (m_IsUnderwater)
        {
            ApplyBuoyancy();
        }
    }

    private void ApplyBuoyancy()
    {
        var waveHeight = CalculateWaveHeight(transform.position.x, transform.position.y);
        var submersionDepth = waveHeight - transform.position.y;

        if (submersionDepth > 0)
        {
            var force = Mathf.Min(submersionDepth * m_Buoyancy, m_MaxBuoyancy);
            var buoyancyForce = force * Vector3.up;

            m_RigidBody.AddForce(buoyancyForce, ForceMode.Acceleration);
        }
    }

    private float CalculateWaveHeight(float x, float y)
    {
        var wave = Mathf.Sin(((x + y) * m_Frequency) + (Time.time * m_Speed));
        return wave + m_WaterLevel;
    }
}