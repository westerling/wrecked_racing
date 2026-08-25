using System.Collections;
using UnityEngine;

public class DrumBomb : Ammunition
{
    [SerializeField]
    private float m_Radius = 8f;

    [SerializeField]
    private float m_Power = 300000f;

    [Header("Sounds")]
    [SerializeField]
    private Sound m_ExplosionSound;

    private bool m_Active;

    private void Awake()
    {
        AmmunitionType = AmmunitionType.Drumbomb;
    }

    public void ActivateBarrel()
    {
        StartCoroutine(ActivateAfterDelay());
    }

    protected IEnumerator ActivateAfterDelay()
    {
        m_Active = false;
        yield return new WaitForSeconds(0.5f);
        m_Active = true;
    }

    protected virtual void Explode()
    {
        var explosionPos = transform.position;
        var colliders = Physics.OverlapSphere(explosionPos, m_Radius);

        var explosion = FxPool.Current.GetPooledObjectOfType(ParticleType.Explosion_m);

        if (explosion != null)
        {
            explosion.transform.position = explosionPos;
            explosion.SetActive(true);
        }

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent<Rigidbody>(out var rigidbody))
            {
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 3000f);
            }

            if (hit.TryGetComponent(out Health health))
            {
                health.Damage(48);
            }
        }

        m_Active = false;

        SoundFxManager.Current.PlaySoundClip(m_ExplosionSound, transform);
        Deactivate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!m_Active)
        {
            return;
        }

        Explode();
    }
}
