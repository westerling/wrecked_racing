using UnityEngine;

public class Differential : CarComponent
{
    [SerializeField]
    private Wheel m_LeftWheel;

    [SerializeField]
    private Wheel m_RightWheel;

    private float m_TorqueSplit = 0.5f;
    private float m_LimitedSlipRatio = 2.0f;

    public Wheel LeftWheel
    {
        get => m_LeftWheel;
        set => m_LeftWheel = value;
    }
    
    public Wheel RightWheel
    {
        get => m_RightWheel;
        set => m_RightWheel = value;
    }

    public void ApplyTorque(float totalTorque)
    {
        if (LeftWheel.WheelCollider.GetGroundHit(out var hitLeft) && RightWheel.WheelCollider.GetGroundHit(out var hitRight))
        {
            var slipLeft = Mathf.Abs(hitLeft.forwardSlip);
            var slipRight = Mathf.Abs(hitRight.forwardSlip);
            
            if (slipLeft > slipRight * m_LimitedSlipRatio)
            {
                m_TorqueSplit = 0.7f;
            }
            else if (slipRight > slipLeft * m_LimitedSlipRatio)
            {
                m_TorqueSplit = 0.3f;
            }
            else
            {
                m_TorqueSplit = 0.5f;
            }

            LeftWheel.MotorTorque = totalTorque * m_TorqueSplit;
            RightWheel.MotorTorque = totalTorque * (1 - m_TorqueSplit);
        }
    }
}
