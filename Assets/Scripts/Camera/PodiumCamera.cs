using Unity.Cinemachine;

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
            if (target.Object.gameObject.TryGetComponent(out TVScreen tvScreen))
            {
                if (tvScreen.FocusPoint != null)
                {
                    m_Camera.Follow = tvScreen.FocusPoint.transform;
                    m_Camera.LookAt = tvScreen.FocusPoint.transform;
                }
            }
        }
    }

    private void OnDestroy()
    {
        m_CameraControl.TargetGroupChanged -= OnTargetGroupChanged;
    }
}
