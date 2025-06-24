using System.Collections;
using UnityEngine;

public class Mine : Ammunition
{
    private float m_Radius = 10f;
    private float m_Power = 250000f;
    private float m_SafeTimer = 2f;

    private bool m_Active = false;

    private void Awake()
    {
        WeaponType = WeaponType.Mine;
    }

    public void PlaceMine()
    {
        StartCoroutine(SetMineSafety());
    }

    private IEnumerator SetMineSafety()
    {
        m_Active = false;
        yield return new WaitForSeconds(m_SafeTimer);
        m_Active = true;
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
                hitRigidBody.AddExplosionForce(m_Power, explosionPos, m_Radius, 20000f);
            }

        }

        Deactivate();
    }
}
