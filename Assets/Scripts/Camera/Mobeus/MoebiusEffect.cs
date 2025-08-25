using UnityEngine;


[ExecuteInEditMode]
public class MoebiusEffect : MonoBehaviour
{
    [SerializeField]
    private Material m_Material;
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (m_Material != null)
        {
            Graphics.Blit(src, dest, m_Material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
