using System.Collections;
using UnityEngine;

public abstract class Missile : Ammunition
{
    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private Transform m_RocketTrailOrigin;

    [Header("Blast")]
    [SerializeField]
    private float m_Radius = 5f;
    [SerializeField]
    private float m_Power = 100000f;

    [Header("Sounds")]
    [SerializeField]
    private Sound m_ExplosionSound;

    [SerializeField]
    private Sound m_TrailSound;

    private GameObject m_RocketTrail;

    private bool m_Exploded = false;
    private bool m_Active;
    private float m_Speed;
    private float m_TopSpeed;
    private float m_AccelerationTimer;
    protected const float ACCELERATION_TIME = 3f;


    public float TopSpeed
    {
        get => m_TopSpeed;
        set => m_TopSpeed = value;
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

    protected float Speed
    {
        get => m_Speed;
        set => m_Speed = value;
    }

    protected float AccelerationTimer
    {
        get => m_AccelerationTimer;
        set => m_AccelerationTimer = value;
    }

    protected Sound TrailSound
    {
        get => m_TrailSound;
    }

    protected bool Exploded
    {
        get => m_Exploded;
        set => m_Exploded = value;
    }

    private void FixedUpdate()
    {
        UpdatePosition();
    }

    protected abstract void UpdatePosition();

    public void AddPooledObject()
    {
        var pooledObject = FxPool.Current.GetPooledObjectOfType(ParticleType.RocketTrail);

        if (pooledObject != null)
        {
            pooledObject.SetActive(true);
            pooledObject.transform.parent = m_RocketTrailOrigin.transform;
            pooledObject.transform.SetPositionAndRotation(m_RocketTrailOrigin.position, m_RocketTrailOrigin.rotation);

            if (pooledObject.TryGetComponent(out RocketTrail rocketTrail))
            {
                m_RocketTrail = rocketTrail.gameObject;
            }
        }
    }

    protected IEnumerator ActivateAfterDelay()
    {
        TopSpeed = TopSpeed + 3;
        Active = false;
        yield return new WaitForSeconds(0.5f);
        Active = true;
        TopSpeed = TopSpeed + 0.2f;
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

            if (hit.TryGetComponent(out Health health))
            {
                health.Damage(29);
            }
        }

        if (m_RocketTrail != null)
        {
            if (m_RocketTrail.TryGetComponent(out RocketTrail rocketTrail))
            {
                rocketTrail.ReleaseGameObject();
            }
        }

        SoundFxManager.Current.PlaySoundClip(m_ExplosionSound, transform);
        Deactivate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Active)
        {
            return;
        }

        Exploded = true;
        Explode();
    }
}
