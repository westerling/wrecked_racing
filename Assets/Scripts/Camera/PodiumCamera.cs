using Cinemachine;
using UnityEngine;

public class PodiumCamera : MonoBehaviour
{
    private CinemachineVirtualCamera m_PodiumCamera;

    private void Awake()
    {
        if (gameObject.TryGetComponent(out CameraControl cameraControl))
        {
            m_PodiumCamera = cameraControl.PodiumCamera;
        }
    }

    public void SetTarget(GameObject podium)
    {
        m_PodiumCamera.Follow = podium.transform;
        m_PodiumCamera.LookAt = podium.transform;
    }
}
