using System.Collections;
using UnityEngine;

public class Flames : ParticleEffect
{
    protected override void OnEnable()
    {
        EmitParticles(false);
    }

    public void EmitParticles(bool active)
    {
        foreach (var ParticleSystem in ParticleSystems)
        {
            if (active)
            {
                ParticleSystem.Play();
            }
            else
            {
                ParticleSystem.Stop();
            }
        }
    }

    public void ReleaseGameObject()
    {
        EmitParticles(false);
        StartCoroutine(DeactivateAfterPlay());
    }
}
