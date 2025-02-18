using UnityEngine;

public class Wheel : MonoBehaviour
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

    public WheelCollider WheelCollider 
    {
        get => m_WheelCollider;  
    }

    public WheelPlacement WheelPlacement
    {
        get => m_WheelPlacement;
    }

    public Transform GraphicsTransform
    {
        get => m_GraphicsTransform;
    }

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
        WheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        GraphicsTransform.position = position;
        GraphicsTransform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        WheelCollider.steerAngle = SteerAngle;
        WheelCollider.motorTorque = MotorTorque;
        WheelCollider.brakeTorque = BrakeTorque;
    }
}
