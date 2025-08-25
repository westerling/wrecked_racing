using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class EnableDepthNormals : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera;

    private void OnEnable()
    {
        m_Camera.depthTextureMode = DepthTextureMode.DepthNormals;
    }
}
