using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineSplineDolly))]
public class DollyCamera : BaseCamera
{
    public bool RotationTest;

    private float m_MinFOV = 40f;
    private float m_MaxFOV = 70f;
    private float m_ZoomSpeed = 2f;
    private float m_TurnAheadDistance = 5f;
    private float m_OffsetSmoothSpeed = 5f;

    private Vector3 m_BaseOffset = new Vector3(0f, 40f, -40f);
    private Vector3 m_DynamicOffset;

    private Transform m_Leader;
    private CinemachineSplineDolly m_SplineDolly;
    private CinemachineTargetGroup m_TargetGroup;

    protected override void Awake()
    {
        base.Awake();

        m_CameraControl.TargetGroupChanged += OnTargetGroupChanged;
        RaceManager.Current.LeaderChanged += OnLeaderChanged;
    }

    private void Start()
    {
        SetupDolly();
    }

    private void Update()
    {
        if (m_TargetGroup == null || m_Camera == null)
        {
            return;
        }

        AdjustZoom();
        AdjustOffset();
    }

    void LateUpdate()
    {
        if (m_TargetGroup == null || m_Camera == null)
        {
            return;
        }

        RotateCamera();
    }

    private void SetupDolly()
    {
        var spline = RaceManager.Current.DollyTrack;

        if (TryGetComponent(out CinemachineSplineDolly splineDolly))
        {
            splineDolly.Spline = spline;
        }

        m_SplineDolly = m_Camera.GetComponent<CinemachineSplineDolly>();
    }

    private void RotateCamera()
    {
        var direction = m_TargetGroup.transform.position - m_Camera.transform.position;

        if (direction != Vector3.zero)
        {
            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            m_Camera.transform.rotation = Quaternion.Slerp(m_Camera.transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void AdjustOffset()
    {
        if (m_Leader != null)
        {
            var aheadOffset = m_Leader.forward * m_TurnAheadDistance;
            m_DynamicOffset = Vector3.Lerp(m_DynamicOffset, m_BaseOffset + aheadOffset, Time.deltaTime * m_OffsetSmoothSpeed);

            if (m_SplineDolly != null)
            {
                m_SplineDolly.SplineOffset = m_DynamicOffset;
            }
        }
    }

    private void AdjustZoom()
    {
        var groupBounds = CalculateTargetBounds();
        var targetFOV = Mathf.Lerp(m_MinFOV, m_MaxFOV, groupBounds.size.magnitude / 50f);
        m_Camera.Lens.FieldOfView = Mathf.Lerp(m_Camera.Lens.FieldOfView, targetFOV, Time.deltaTime * m_ZoomSpeed);
    }

    private Bounds CalculateTargetBounds()
    {
        var bounds = new Bounds(m_TargetGroup.transform.position, Vector3.zero);
        
        foreach (var target in m_TargetGroup.Targets)
        {
            bounds.Encapsulate(target.Object.position);
        }

        return bounds;
    }

    private void OnTargetGroupChanged(CinemachineTargetGroup targetGroup)
    {
        m_TargetGroup = targetGroup;

        m_Camera.Follow = targetGroup.transform;
        m_Camera.LookAt = targetGroup.transform;
    }

    private void OnLeaderChanged(GameObject newLeader)
    {
        m_Leader = newLeader.transform;
    }

    private void OnDestroy()
    {
        m_CameraControl.TargetGroupChanged -= OnTargetGroupChanged;
        RaceManager.Current.LeaderChanged -= OnLeaderChanged;
    }
}
