using System.Collections.Generic;
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
    private WheelPlacement m_WheelPlacement;

    [SerializeField]
    private AudioSource m_AudioSource;
    
    private AnimationCurve m_AnimationCurve;
    private WheelSurfaceData m_CurrentSurfaceData;
    private readonly List<StickySurfaceTimer> m_StickySurfaces = new();

    private PooledSkidTrail m_TrailRenderer;

    private CarStatus m_CarStatus;

    private float m_SteerAngle;
    private float m_MotorTorque;
    private float m_BrakeTorque;
    private float m_WheelRpm;

    private bool m_ForceBrake;
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
        SetSlipCurve();
    }

    private void Update()
    {
        UpdateStickySurfaces();
        UpdateWheelEffects();
        ApplySurfaceForces();
        CheckForForcedBrake();

        WheelRpm = m_WheelCollider.rpm;
        WheelCollider.GetWorldPose(out Vector3 position, out Quaternion rotation);
        GraphicsTransform.position = position;
        GraphicsTransform.rotation = rotation;
    }

    private void CheckForForcedBrake()
    {
        m_ForceBrake = false;

        if (m_CurrentSurfaceData != null)
        {
            if (m_CurrentSurfaceData.SpeedPercentage < 1)
            {
                if (Car.CurrentSpeedRatio > m_CurrentSurfaceData.SpeedPercentage)
                {
                    m_ForceBrake = true;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        var grip = m_CurrentSurfaceData != null
        ? m_CurrentSurfaceData.GripMultiplier
        : 1f;

        grip *= GetStickyGripMultiplier();

        var speedEffect = m_CurrentSurfaceData != null
        ? m_CurrentSurfaceData.SpeedPercentage
        : 1f;

        WheelCollider.steerAngle = SteerAngle;
        WheelCollider.motorTorque = MotorTorque * grip * speedEffect;
        WheelCollider.brakeTorque = m_ForceBrake ? 500f : BrakeTorque * grip;
    }

    private void SetSlipCurve()
    {
        switch (WheelPlacement)
        {
            case WheelPlacement.FrontLeft:
            case WheelPlacement.FrontRight:
                m_AnimationCurve = Car.Stats.SlipCurveFront;
                break;
            case WheelPlacement.RearLeft:
            case WheelPlacement.RearRight:
                m_AnimationCurve = Car.Stats.SlipCurveRear;
                break;
        }
    }

    private void UpdateStickySurfaces()
    {
        for (var i = m_StickySurfaces.Count - 1; i >= 0; i--)
        {
            m_StickySurfaces[i].TimeLeft -= Time.deltaTime;

            if (m_StickySurfaces[i].TimeLeft <= 0)
            {
                m_StickySurfaces.RemoveAt(i);
            }
        }
    }

    private void UpdateWheelEffects()
    {
        if (m_CarStatus != CarStatus.Active ||
            !m_WheelCollider.GetGroundHit(out var hit))
        {
            DisableEffects();
            return;
        }

        var emitTrail = false;

        m_CurrentSurfaceData = GetSurfaceData(hit);

        if (m_CurrentSurfaceData != null && m_CurrentSurfaceData.IsSticky)
        {
            AddOrRefreshStickySurface(m_CurrentSurfaceData);
            emitTrail = true;
        }

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

        UpdateAudio(slipRatio);

        if (m_CurrentSurfaceData != null && m_TrailRenderer != null)
        {
            if (m_CurrentSurfaceData.SurfaceType != m_TrailRenderer.SurfaceType)
            {
                RemoveOldRenderer();
            }
        }

        if (m_TrailRenderer == null)
        {
            AddNewTrailRenderer();   
        }

        m_TrailRenderer.EmitTrail(combinedSlip > slipThreshold);
    }

    private void AddOrRefreshStickySurface(WheelSurfaceData surface)
    {
        var existing = m_StickySurfaces.Find(s => s.Surface == surface);

        if (existing != null)
        {
            existing.TimeLeft = surface.StickyTimer;
        }
        else
        {
            m_StickySurfaces.Add(new StickySurfaceTimer
            {
                Surface = surface,
                TimeLeft = surface.StickyTimer,
                Duration = surface.StickyTimer
            });
        }
    }

    private void RemoveOldRenderer()
    {
        if (m_TrailRenderer == null)
        {
            return;
        }

        m_TrailRenderer.StopTrail();
        m_TrailRenderer = null;
    }

    private void AddNewTrailRenderer()
    {
        var pooledObject = TrailPool.Current.GetPooledObjectOfType(m_CurrentSurfaceData.SurfaceType);

        if (pooledObject == null)
        {
            Debug.LogError("No pooled trail object exist of type: " + m_CurrentSurfaceData.SurfaceType);
            return;
        }

        pooledObject.transform.SetParent(transform);
        pooledObject.transform.position = m_FxOrigin.position;
        pooledObject.SetActive(true);

        if (pooledObject.TryGetComponent(out PooledSkidTrail pooledTrailedRenderer))
        {
            m_TrailRenderer = pooledTrailedRenderer;
        }
    }

    private void ApplySurfaceForces()
    {
        if (m_CurrentSurfaceData == null || m_CurrentSurfaceData.SideForceStrength <= 0)
        {
            return;
        }

        if (Random.value > m_CurrentSurfaceData.SideForceFrequency * Time.deltaTime)
        {
            return;
        }

        var direction = Random.value > 0.5f ? 1f : -1f;
        var force = transform.right * direction *
                        m_CurrentSurfaceData.SideForceStrength;

        Car.Rigidbody.AddForce(
            force,
            ForceMode.Force);
    }

    private WheelSurfaceData GetSurfaceData(WheelHit hit)
    {
        if (hit.collider.TryGetComponent(out GroundSurface groundSurface))
        {
            return groundSurface.SurfaceData;
        }

        return null;
    }

    private void DisableEffects()
    {
        if (m_TrailRenderer != null)
        {
            m_TrailRenderer.EmitTrail(false);
        }

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

    private float GetStickyGripMultiplier()
    {
        float grip = 1f;

        foreach (var sticky in m_StickySurfaces)
        {
            float strength = sticky.Strength;

            float surfaceGrip = sticky.Surface.GripMultiplier;

            grip *= Mathf.Lerp(1f, surfaceGrip, strength);
        }

        return grip;
    }

    private void AddListeners()
    {
        Car.Health.CarHealthStatus += OnCarStatusChanged;
    }

    private void OnCarStatusChanged(CarStatus carStatus, Car car)
    {
        m_CarStatus = carStatus;
    }

    private void RemoveListeners()
    {
        Car.Health.CarHealthStatus -= OnCarStatusChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
