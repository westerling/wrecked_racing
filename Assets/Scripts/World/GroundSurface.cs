using UnityEngine;

public class GroundSurface : MonoBehaviour
{
    [SerializeField]
    private WheelSurfaceData m_SurfaceData;

    public WheelSurfaceData SurfaceData
    {
        get => m_SurfaceData;
    }
}
