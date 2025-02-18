using Cinemachine;
using UnityEngine;

public class CountdownCamera : MonoBehaviour
{
    private CinemachineVirtualCamera m_CountdownCamera;

    private void Awake()
    {
        if (gameObject.TryGetComponent(out CameraControl cameraControl))
        {
            m_CountdownCamera = cameraControl.CountdownCamera;
        }
    }

    public void SetTarget(GameObject target)
    {
        m_CountdownCamera.Follow = target.transform;
        m_CountdownCamera.LookAt = target.transform;
    }
}
