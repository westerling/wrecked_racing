using System.Collections;
using UnityEngine;

public class ParticleEffect : SpecialEffect
{
    [SerializeField]
    private ParticleSystem[] m_ParticleSystems;

    private void OnEnable()
    {
        foreach (var particleSystem in m_ParticleSystems)
        {
            particleSystem.Play(true);
        }

        StartCoroutine(DeactivateAfterPlay());
    }

    private IEnumerator DeactivateAfterPlay()
    {
        yield return new WaitWhile(() => AnyParticlesAlive());

        gameObject.transform.SetParent(FxPool.Current.transform);
        gameObject.SetActive(false);
    }

    private bool AnyParticlesAlive()
    {
        foreach (var particleSystem in m_ParticleSystems)
        {
            if (particleSystem.IsAlive(true))
            {
                return true;
            }
        }

        return false;
    }
}
