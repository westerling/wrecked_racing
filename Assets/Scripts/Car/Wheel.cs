using System;
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

    [SerializeField]
    private AudioSource m_AudioSource;
    
    private AnimationCurve m_AnimationCurve;
    private WheelSurfaceData m_CurrentSurfaceData;

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
        UpdateWheelEffects();

        WheelRpm = m_WheelCollider.rpm;
        WheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        GraphicsTransform.position = position;
        GraphicsTransform.rotation = rotation;
    }

    private void FixedUpdate()
    {
        var grip = m_CurrentSurfaceData != null
            ? m_CurrentSurfaceData.GripMultiplier
            : 1f;

        WheelCollider.steerAngle = SteerAngle;
        WheelCollider.motorTorque = MotorTorque * grip;
        WheelCollider.brakeTorque = BrakeTorque * grip;
    }

    private void UpdateWheelEffects()
    {
        if (m_CarStatus != CarStatus.Active ||
            !m_WheelCollider.GetGroundHit(out var hit))
        {
            DisableEffects();
            return;
        }

        m_CurrentSurfaceData = GetSurfaceData(hit);

        var combinedSlip = Mathf.Sqrt(
            hit.forwardSlip * hit.forwardSlip +
            hit.sidewaysSlip * hit.sidewaysSlip);

        var slipThreshold = m_AnimationCurve.Evaluate(Car.CurrentSpeedRatio);

        if (m_CurrentSurfaceData != null)
        {
            slipThreshold *= m_CurrentSurfaceData.SlipSensitivity;
        }

        var slipRatio = Mathf.InverseLerp(slipThreshold, slipThreshold * 2f, combinedSlip);
        slipRatio = Mathf.Clamp01(slipRatio);

        m_TrailRenderer.emitting = combinedSlip > slipThreshold;

        UpdateAudio(slipRatio);

        if (m_CurrentSurfaceData?.TrailMaterial != null &&
            m_TrailRenderer.material != m_CurrentSurfaceData.TrailMaterial)
        {
            m_TrailRenderer.material = m_CurrentSurfaceData.TrailMaterial;
        }
    }

    private WheelSurfaceData GetSurfaceData(WheelHit hit)
    {
        var ground = hit.collider.GetComponent<GroundSurface>();
        return ground != null ? ground.SurfaceData : null;
    }

    private void DisableEffects()
    {
        m_TrailRenderer.emitting = false;
        if (m_AudioSource.isPlaying)
        {
            m_AudioSource.Stop();
        } 
    }

    private void UpdateAudio(float slipRatio)
    {
        if (m_CurrentSurfaceData == null || slipRatio < 0.05f)
        {
            m_AudioSource.volume = Mathf.Lerp(m_AudioSource.volume, 0f, Time.deltaTime * 3f);
            
            if (m_AudioSource.volume < 0.05f && m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }
                
            return;
        }

        if (!m_AudioSource.isPlaying)
        {
            if (m_AudioSource.clip != m_CurrentSurfaceData.SkidSound)
            {
                m_AudioSource.clip = m_CurrentSurfaceData.SkidSound;
            }

            Debug.Log("PLAY");
            m_AudioSource.Play();
        }

        m_AudioSource.volume = Mathf.Lerp(
            m_AudioSource.volume,
            slipRatio * m_CurrentSurfaceData.VolumeMultiplier,
            Time.deltaTime * 5f);

        m_AudioSource.pitch = Mathf.Lerp(
            m_AudioSource.pitch,
            (0.8f + slipRatio * 0.4f) * m_CurrentSurfaceData.PitchMultiplier,
            Time.deltaTime * 5f);
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

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
