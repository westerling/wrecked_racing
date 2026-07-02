using System.Collections;
using UnityEngine;

public class Barrel : Ammunition
{
    [SerializeField]
    private float m_Radius = 5f;

    [SerializeField]
    private float m_Power = 100000f;

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
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 300f);
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
