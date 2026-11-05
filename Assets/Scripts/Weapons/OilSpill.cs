
using System.Collections;
using UnityEngine;

public class OilSpill : Ammunition
{
    [SerializeField]
    private MeshFilter m_MeshFilter;

    [SerializeField]
    private MeshCollider m_MeshCollider;

    [SerializeField]
    private float m_SurfaceOffset = 0.04f;

    private float m_LifeTime = 0;
    private bool m_IsDeactivating = false;

    private Mesh m_Mesh;
    private Vector3[] m_OriginalVertices;

    private void Awake()
    {
        AmmunitionType = AmmunitionType.Oil;

        m_Mesh = m_MeshFilter.mesh;
        m_OriginalVertices = m_Mesh.vertices;
    }

    private void Update()
    {
        if (m_IsDeactivating)
        {
            return;
        }

        m_LifeTime += Time.deltaTime;

        if (m_LifeTime > 60f)
        {
            m_IsDeactivating = true;
            StartCoroutine(DeactivateRoutine());
        }
    }

    public void SetupOilSpill()
    {
        StopAllCoroutines();

        m_LifeTime = 0;
        transform.localScale = Vector3.one;

        m_Mesh.vertices = m_OriginalVertices;
        m_Mesh.RecalculateNormals();
        m_Mesh.RecalculateBounds();
        
        m_MeshCollider.sharedMesh = null;

        SetVerticies();

        StartCoroutine(ScaleMeshTo(0.5f, 1f, 3f));
    }

    private void SetVerticies()
    {
        var mesh = m_MeshFilter.mesh;
        var vertices = mesh.vertices;

        for (var i = 0; i < vertices.Length; i++)
        {
            var worldPosition = transform.TransformPoint(vertices[i]);
            var rayOrigin = worldPosition + Vector3.up * 5f;

            if (Physics.Raycast(
                rayOrigin,
                Vector3.down,
                out var hit,
                20f,
                LayerMasks.DriveableLayerMask))
            {
                vertices[i] = transform.InverseTransformPoint(
                    hit.point + hit.normal * m_SurfaceOffset
                );
            }
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        m_MeshCollider.sharedMesh = null;
        m_MeshCollider.sharedMesh = mesh;
    }

    private IEnumerator DeactivateRoutine()
    {
        yield return ScaleMeshTo(1f, 0f, 3f);

        Deactivate();
        m_IsDeactivating = false;
    }

    private IEnumerator ScaleMeshTo(float startScale, float targetScale, float duration)
    {
        var mesh = m_MeshFilter.mesh;
        var targetVertices = mesh.vertices;

        var center = Vector3.zero;

        foreach (var vertex in targetVertices)
            center += vertex;

        center /= targetVertices.Length;

        var timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            var t = Mathf.Clamp01(timer / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            var scale = Mathf.Lerp(startScale, targetScale, t);
            var vertices = new Vector3[targetVertices.Length];

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] = center + (targetVertices[i] - center) * scale;
            }

            mesh.vertices = vertices;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            yield return null;
        }

        m_MeshCollider.sharedMesh = null;
        m_MeshCollider.sharedMesh = mesh;
    }
}
