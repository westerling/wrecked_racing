using System.Collections;
using UnityEngine;

public class Mine : Ammunition
{
    [Header("Sounds")]
    [SerializeField]
    private Sound m_ExplosionSound;

    [SerializeField]
    private GameObject m_GreenLight;

    [SerializeField]
    private GameObject m_RedLight;

    [SerializeField]
    private GameObject m_NeutralLight;

    private float m_Radius = 10f;
    private float m_SafeTimer = 2f;
    private float m_FlashTimer = 1f;
    private bool m_Active = false;

    private void Awake()
    {
        AmmunitionType = AmmunitionType.Mine;
    }

    private void Update()
    {
        if (m_Active)
        {
            m_FlashTimer -= Time.deltaTime;

            if (m_FlashTimer < 0)
            {
                m_RedLight.SetActive(!m_RedLight.activeInHierarchy);
                m_NeutralLight.SetActive(!m_NeutralLight.activeInHierarchy);

                m_FlashTimer = 1f;
            }
        }
    }

    public void PlaceMine()
    {
        StartCoroutine(SetMineSafety());
    }

    private IEnumerator SetMineSafety()
    {
        m_GreenLight.SetActive(true);
        m_RedLight.SetActive(false);
        m_NeutralLight.SetActive(false);

        m_Active = false;
        yield return new WaitForSeconds(m_SafeTimer);
        m_Active = true;

        m_GreenLight.SetActive(false);
        m_RedLight.SetActive(true);
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
