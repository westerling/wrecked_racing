using UnityEngine;

public class FlameThrower : Weapon
{
    [SerializeField]
    private Transform m_FlameOrigin;

    [Header("Sounds")]
    [SerializeField]
    private Sound m_SoundEffect;

    private bool m_FlamesActive = false;

    private Flames m_Flames;

    protected override void Fire()
    {
        AddImpact();
        SoundFxManager.Current.PlaySoundClip(m_SoundEffect, transform);

        if (!m_FlamesActive)
        {
            GetGameObjectFromPool();
            m_Flames.EmitParticles(true);
            AddConditionalModifiers();
            m_FlamesActive = true;
        }
    }

    protected override void ReturnWeapon()
    {
        m_FlamesActive = false;

        if (m_Flames != null)
        {
            m_Flames.gameObject.transform.SetParent(null);
            m_Flames.ReleaseGameObject();
            m_Flames = null;
        }
    }

    private void AddImpact()
    {
        if (Physics.Raycast(m_FlameOrigin.position, m_FlameOrigin.forward, out var hit, 5f, LayerMasks.ShootableLayerMask))
        {
            if (hit.rigidbody != null && hit.transform != ParentCar.transform)
            {
                ApplyImpactForce(hit.rigidbody, m_FlameOrigin.forward, 10000f);

                if (hit.collider.TryGetComponent(out Health health))
                {
                    health.Damage(10);
                }
            }
        }
    }

    private void ApplyImpactForce(Rigidbody rigidbody, Vector3 forwardDirection, float forceAmount)
    {
        var totalForce = forwardDirection.normalized * forceAmount;
        rigidbody.AddForce(totalForce, ForceMode.Impulse);
    }

    protected override void Update()
    {
        base.Update();

        if (!IsFiring)
        {
            if (m_FlamesActive)
            {
                if (m_Flames != null)
                {
                    m_Flames.EmitParticles(false);
                    m_FlamesActive = false;
                }
            }
        }
    }

    private void GetGameObjectFromPool()
    {
        var pooledObject = FxPool.Current.GetPooledObjectOfType(ParticleType.CastedFlames);

        if (pooledObject == null)
        {
            return;
        }

        var transform = pooledObject.transform;

        transform.SetParent(m_FlameOrigin, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        pooledObject.SetActive(true);

        m_Flames = pooledObject.GetComponent<Flames>();
    }

    private void AddConditionalModifiers()
    {
        ParentCar.StatusManager.AddConditionalModifier(
            Stat.Speed,
            ModifierType.Multiplier,
            () => 1.1f,
            () => IsFiring == true);

        ParentCar.StatusManager.AddConditionalModifier(
           Stat.Acceleration,
           ModifierType.Multiplier,
           () => 1.1f,
           () => IsFiring == true);
    }
}
