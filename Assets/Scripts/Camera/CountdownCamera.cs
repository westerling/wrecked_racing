using UnityEngine;

public class CountdownCamera : BaseCamera
{
    protected override void Awake()
    {
        base.Awake();
        m_CameraControl.TargetChanged += OnTargetChanged;
    }

    private void OnTargetChanged(GameObject target)
    {
        m_Camera.Follow = target.transform;
        m_Camera.LookAt = target.transform;
    }

    private void OnDestroy()
    {
        m_CameraControl.TargetChanged -= OnTargetChanged;
    }
}
