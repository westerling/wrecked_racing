using UnityEngine;

public class Wheel : CarComponent
{
    [SerializeField]
    private WheelCollider m_WheelCollider;

    [SerializeField]
    private Transform m_GraphicsTransform;

    [SerializeField]
    private Transform m_FxOrigin;

    [SerializeField]
    private TrailRenderer m_TrailRenderer;

    [SerializeField]
    private WheelPlacement m_WheelPlacement;

    private AnimationCurve m_AnimationCurve;
    
    private CarStatus m_CarStatus;

    private float m_SteerAngle;
    private float m_MotorTorque;
    private float m_BrakeTorque;
    private float m_WheelRpm;

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

    public float WheelRpm
    {
        get => m_WheelRpm;
        private set => m_WheelRpm = value;
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

    protected override void Awake()
    {
        base.Awake();

        AddListeners();

        if (WheelPlacement == WheelPlacement.FrontLeft || WheelPlacement == WheelPlacement.FrontRight)
        {
            m_AnimationCurve = Car.Stats.SlipCurveFront;
        }
        else if (WheelPlacement == WheelPlacement.RearLeft || WheelPlacement == WheelPlacement.RearRight)
        {
            m_AnimationCurve = Car.Stats.SlipCurveRear;
        }
    }

    private void Update()
    {
        AddSkidmarks();

        WheelRpm = m_WheelCollider.rpm;
        WheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        GraphicsTransform.position = position;
        GraphicsTransform.rotation = rotation;
    }

    private void AddListeners()
    {
        Car.CarStatusChanged += OnCarStatusChanged;
    }

    private void OnCarStatusChanged(Car car, CarStatus carStatus)
    {
        m_CarStatus = carStatus;
    }

    private void RemoveListeners()
    {
        Car.CarStatusChanged -= OnCarStatusChanged;
    }

    private void AddSkidmarks()
    {
        if (m_CarStatus == CarStatus.Active)
        {
            if (m_WheelCollider.GetGroundHit(out var hit))
            {
                var combinedSlip = Mathf.Sqrt(hit.forwardSlip * hit.forwardSlip + hit.sidewaysSlip * hit.sidewaysSlip);

                m_TrailRenderer.emitting = Mathf.Abs(combinedSlip) > m_AnimationCurve.Evaluate(Car.CurrentSpeedRatio);
            }
            else
            {
                m_TrailRenderer.emitting = false;
            }
        }
        else
        {
            m_TrailRenderer.emitting = false;
        }
    }

    private void FixedUpdate()
    {
        WheelCollider.steerAngle = SteerAngle;
        WheelCollider.motorTorque = MotorTorque;
        WheelCollider.brakeTorque = BrakeTorque;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
