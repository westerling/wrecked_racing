using UnityEngine;

[CreateAssetMenu(fileName = "WheelSurfaceData", menuName = "Scriptable Objects/WheelSurfaceData")]
public class WheelSurfaceData : ScriptableObject
{

    [Header("Handling / Grip")]
    [Tooltip("0 = no traction, 1 = full traction (asphalt)")]
    [Range(0.1f, 1f)]
    [SerializeField]
    private float m_GripMultiplier = 1f;

    [Tooltip("How quickly traction breaks loose on this surface")]
    [Range(0.1f, 2f)]
    [SerializeField]
    private float m_SlipSensitivity = 1f;  

    [Header("Audio")]
    [SerializeField]
    private AudioClip m_SkidSound;
    
    [SerializeField]
    private float m_VolumeMultiplier = 1f;
    
    [SerializeField]
    private float m_PitchMultiplier = 1f;

    [Header("Trails")]
    [SerializeField]
    private Material m_TrailMaterial;
        
    [Header("Particles")]
    [SerializeField]
    private GameObject m_DustFxPrefab;

    public AudioClip SkidSound
    {
        get => m_SkidSound;
    }

    public float GripMultiplier
    {
        get => m_GripMultiplier;
    }
    
    public GameObject DustFxPrefab
    {
        get => m_DustFxPrefab;
    }

    public Material TrailMaterial
    {
        get => m_TrailMaterial;
    }
    
    public float PitchMultiplier
    {
        get => m_PitchMultiplier;
    }

    public float VolumeMultiplier
    {
        get => m_VolumeMultiplier;
    }

    public float SlipSensitivity
    {
        get => m_SlipSensitivity;
    }
}
