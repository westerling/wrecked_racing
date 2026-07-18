using System.Collections;
using UnityEngine;

public class PooledSkidTrail : MonoBehaviour
{
    [SerializeField]
    private SurfaceType m_SurfaceType;

    [SerializeField]
    private TrailRenderer m_TrailRenderer;

    [SerializeField]
    private ParticleSystem m_ParticleSystem;

    public SurfaceType SurfaceType
    {
        get => m_SurfaceType;
    }
    
    public TrailRenderer TrailRenderer
    {
        get => m_TrailRenderer;
    }

    public void EmitTrail(bool emit)
    {
        m_TrailRenderer.emitting = emit;
        m_ParticleSystem.Play();
    }

    public void StopTrail()
    {
        m_TrailRenderer.emitting = false;
        m_ParticleSystem.Stop();
        StartCoroutine(ReturnToPool());
    }

    private IEnumerator ReturnToPool()
    {
        gameObject.transform.SetParent(TrailPool.Current.transform);
        yield return new WaitForSeconds(m_TrailRenderer.time);
        m_TrailRenderer.Clear();
        gameObject.SetActive(false);
    }
}
