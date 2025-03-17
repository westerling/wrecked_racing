using System;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class BaseCamera : MonoBehaviour
{
    protected CameraControl m_CameraControl;
    protected CinemachineCamera m_Camera;

    protected virtual void Awake()
    {
        GetCamera();
        GetCameraControl();
    }

    private void GetCameraControl()
    {
        var cameraControl = GetComponentInParent<CameraControl>();

        if (cameraControl == null)
        {
            Debug.LogError("No Camera Control!");
            return;
        }

        m_CameraControl = cameraControl;
    }

    private void GetCamera()
    {
        if (gameObject.TryGetComponent(out CinemachineCamera camera))
        {
            m_Camera = camera;
        }
        else
        {
            Debug.LogError("No Camera!");
        }
    }    
}
