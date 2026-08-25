using System.Collections;
using UnityEngine;

public class Mine : Ammunition
{
    [Header("Sounds")]
    [SerializeField]
    private Sound m_ExplosionSound;

    [SerializeField]
    private GameObject m_MineActive;

    [SerializeField]
    private GameObject m_MineInactive;

    private float m_Radius = 10f;
    private float m_SafeTimer = 2f;

    private bool m_Active = false;

    private void Awake()
    {
        AmmunitionType = AmmunitionType.Mine;
    }

    public void PlaceMine()
    {
        StartCoroutine(SetMineSafety());
    }

    private IEnumerator SetMineSafety()
    {
        SetMineVisual(false);
        m_Active = false;
        yield return new WaitForSeconds(m_SafeTimer);
        m_Active = true;
        SetMineVisual(true);
    }

    private void SetMineVisual(bool active)
    {
        m_MineActive.SetActive(active);
        m_MineInactive.SetActive(!active);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Car _))
        {
            ActivateMine();
        }
    }

    private void ActivateMine()
    {
        if (!m_Active)
        {
            return;
        }

        var explosionPos = transform.position;
        var colliders = Physics.OverlapSphere(explosionPos, m_Radius);

        var pooledObject = FxPool.Current.GetPooledObjectOfType(ParticleType.Explosion_m);

        if (pooledObject != null)
        {
            pooledObject.transform.position = explosionPos;
            pooledObject.SetActive(true);
        }

        foreach (var hit in colliders)
        {
            if (hit.TryGetComponent(out Rigidbody hitRigidBody))
            {
                hitRigidBody.AddExplosionForce(100000f, explosionPos, m_Radius, 500000f);

                var randomTorque = new Vector3(
                    Random.Range(-200f, 200f),
                    Random.Range(-500f, 500f), 
                    Random.Range(-200f, 200f));

                hitRigidBody.AddTorque(randomTorque);
            }

            if (hit.TryGetComponent(out Health health))
            {
                health.Damage(30f);
            }
        }

        SoundFxManager.Current.PlaySoundClip(m_ExplosionSound, transform);
        Deactivate();
    }
}
