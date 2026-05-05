using UnityEngine;

public class SimpleWheel : MonoBehaviour
{
    [SerializeField]
    private WheelCollider m_WheelCollider;

    [SerializeField]
    private Transform m_GraphicsTransform;

    [SerializeField]
    private WheelPlacement m_WheelPlacement;

    private float m_SteerAngle;
    private float m_MotorTorque;
    private float m_BrakeTorque;

    public float SteerAngle
    {
        get => m_SteerAngle;
        set => m_SteerAngle = value;
    }

    public float MotorTorque
    {
        get => m_MotorTorque;
        set => m_MotorTorque = value;
    }

    public float BrakeTorque
    {
        get => m_BrakeTorque;
        set => m_BrakeTorque = value;
    }

    private void Update()
    {
        m_WheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        m_GraphicsTransform.position = position;
        m_GraphicsTransform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        m_WheelCollider.steerAngle = SteerAngle;
        m_WheelCollider.motorTorque = MotorTorque;
        m_WheelCollider.brakeTorque = BrakeTorque;
    }
}
