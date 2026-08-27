public class RocketTrail : ParticleEffect
{
    public void ReleaseGameObject()
    {
        transform.SetParent(null, true);

        foreach (var particleSystem in ParticleSystems)
        {
            particleSystem.Stop();
        }
    }
}
