using Unity.Cinemachine;

public class FollowCamera : BaseCamera
{
    protected override void Awake()
    {
        base.Awake();

        m_CameraControl.TargetGroupChanged += OnTargetGroupChanged;
    }

    private void OnTargetGroupChanged(CinemachineTargetGroup targetGroup)
    {
        m_Camera.Follow = targetGroup.transform;
        m_Camera.LookAt = targetGroup.transform;
    }

    private void OnDestroy()
    {
        m_CameraControl.TargetGroupChanged -= OnTargetGroupChanged;
    }
}
