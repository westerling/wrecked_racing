using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Device;

public class PodiumCamera : BaseCamera
{
    protected override void Awake()
    {
        base.Awake();
        m_CameraControl.TargetGroupChanged += OnTargetGroupChanged;
    }

    private void OnTargetGroupChanged(CinemachineTargetGroup targetGroup)
    {
        foreach (var target in targetGroup.Targets)
        {
            m_Camera.Follow = target.Object;
            m_Camera.LookAt = target.Object;
        }
    }

    private void OnDestroy()
    {
        m_CameraControl.TargetGroupChanged -= OnTargetGroupChanged;
    }
}
