using System.Collections;
using UnityEngine;

public abstract class Missile : Ammunition
{
    [SerializeField]
    private Rigidbody m_Rigidbody;

    [Header("Blast")]
    [SerializeField]
    private float m_Radius = 5f;
    [SerializeField]
    private float m_Power = 100000f;

    private bool m_Active;
    private float m_Speed;

    public float Speed
    {
        get => m_Speed;
        set => m_Speed = value;
    }
    
    protected bool Active
    {
        get => m_Active;
        set => m_Active = value;
    }

    protected Rigidbody RigidBody
    {
        get => m_Rigidbody;
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    protected abstract void UpdatePosition();

    protected IEnumerator ActivateAfterDelay()
    {
        Speed = Speed + 3;
        Active = false;
        yield return new WaitForSeconds(0.5f);
        Active = true;
        Speed = Speed + 0.2f;
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


        SoundFxManager.Current.PlaySoundClip(SoundFxType.Explosion, transform, 80f);
        Deactivate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Active)
        {
            return;
        }

        Explode();
    }
}
