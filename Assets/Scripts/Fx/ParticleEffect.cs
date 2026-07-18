using System.Collections;
using UnityEngine;

public class ParticleEffect : SpecialEffect
{
    [SerializeField]
    private ParticleSystem[] m_ParticleSystems;

    protected ParticleSystem[] ParticleSystems
    {
        get => m_ParticleSystems;
    }

    protected virtual void OnEnable()
    {
        foreach (var particleSystem in ParticleSystems)
        {
            particleSystem.Play(true);
        }

        StartCoroutine(DeactivateAfterPlay());
    }

    protected IEnumerator DeactivateAfterPlay()
    {
        yield return new WaitWhile(() => AnyParticlesAlive());

        gameObject.transform.SetParent(FxPool.Current.transform);
        gameObject.SetActive(false);
    }

    private bool AnyParticlesAlive()
    {
        foreach (var particleSystem in ParticleSystems)
        {
            if (particleSystem.IsAlive(true))
            {
                return true;
            }
        }

        return false;
    }
}
