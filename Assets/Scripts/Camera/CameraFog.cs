using UnityEngine;

public class CameraFog : MonoBehaviour
{
    [SerializeField]
    private bool m_UseDistance = true;

    [SerializeField]
    private Gradient m_DistanceGradient;

    [SerializeField]
    private float m_Near = 0;

    [SerializeField]
    private float m_Far = 100;

    [SerializeField]
    [Range(0, 1)]
    private float m_DistanceFogIntensity = 1.0f;
    
    [SerializeField]
    private bool m_UseDistanceFogOnSky = false;

    [HideInInspector]
    private Material m_Material;

    private Camera m_Camera;
    private Texture2D m_LutDepth;

    private static readonly string ShaderName = "Hidden/FogPlus";
    private static readonly int DistanceLut = Shader.PropertyToID("_DistanceLUT");
    private static readonly int Near = Shader.PropertyToID("_Near");
    private static readonly int Far = Shader.PropertyToID("_Far");
    private static readonly int UseDistanceFog = Shader.PropertyToID("_UseDistanceFog");
    private static readonly int UseDistanceFogOnSky = Shader.PropertyToID("_UseDistanceFogOnSky");
    private static readonly int DistanceFogIntensity = Shader.PropertyToID("_DistanceFogIntensity");
    private static readonly int HeightLut = Shader.PropertyToID("_HeightLUT");
    private static readonly int LowWorldY = Shader.PropertyToID("_LowWorldY");
    private static readonly int HighWorldY = Shader.PropertyToID("_HighWorldY");
    private static readonly int UseHeightFog = Shader.PropertyToID("_UseHeightFog");
    private static readonly int UseHeightFogOnSky = Shader.PropertyToID("_UseHeightFogOnSky");
    private static readonly int HeightFogIntensity = Shader.PropertyToID("_HeightFogIntensity");
    private static readonly int DistanceHeightBlend = Shader.PropertyToID("_DistanceHeightBlend");

    void Awake()
    {
        m_Material = new Material(Shader.Find(ShaderName));
        m_Camera = GetComponent<Camera>();
        m_Camera.depthTextureMode = DepthTextureMode.Depth;
        Debug.Assert(m_Camera.depthTextureMode != DepthTextureMode.None);
    }

    private void Start()
    {
        UpdateShader();
    }

    void OnValidate()
    {
        if (m_Material == null)
        {
            m_Material = new Material(Shader.Find(ShaderName));
        }

        UpdateShader();
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (m_Material == null)
        {
            m_Material = new Material(Shader.Find(ShaderName));
            UpdateShader();
        }

#if UNITY_EDITOR
        UpdateShader();
#endif

        Graphics.Blit(source, destination, m_Material);
    }

    private void UpdateShader()
    {
        UpdateDistanceLut();
        m_Material.SetTexture(DistanceLut, m_LutDepth);
        m_Material.SetFloat(Near, m_Near);
        m_Material.SetFloat(Far, m_Far);
        m_Material.SetFloat(UseDistanceFog, m_UseDistance ? 1f : 0f);
        m_Material.SetFloat(UseDistanceFogOnSky, m_UseDistanceFogOnSky ? 1f : 0f);
        m_Material.SetFloat(DistanceFogIntensity, m_DistanceFogIntensity);
    }

    private void UpdateDistanceLut()
    {
        if (m_DistanceGradient == null)
            return;

        if (m_LutDepth != null)
        {
            DestroyImmediate(m_LutDepth);
        }

        const int width = 256;
        const int height = 1;

        m_LutDepth = new Texture2D(width, height, TextureFormat.RGBA32, /*mipChain=*/false)
        {
            wrapMode = TextureWrapMode.Clamp,
            hideFlags = HideFlags.HideAndDontSave,
            filterMode = FilterMode.Bilinear
        };

        for (var x = 0; x < width; x++)
        {
            var color = m_DistanceGradient.Evaluate(x / (width - 1));
            
            for (var y = 0; y < height; y++)
            {
                m_LutDepth.SetPixel(Mathf.CeilToInt(x), Mathf.CeilToInt(y), color);
            }
        }

        m_LutDepth.Apply();
    }
}
