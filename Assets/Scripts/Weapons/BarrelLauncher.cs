using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BarrelLauncher : Weapon
{
    [SerializeField]
    private Transform[] m_BarrelTransforms;

    private List<GameObject> m_Barrels = new();

    public override void PickupWeapon(Car car)
    {
        base.PickupWeapon(car);

        SetupBarrels();
    }

    protected override void Fire()
    {
        var i = m_Barrels.Count - 1;

        if (m_Barrels[i].TryGetComponent(out Barrel barrel))
        {            
            if (barrel.TryGetComponent(out Rigidbody rigidbody))
            {
                barrel.ActivateBarrel();
                barrel.transform.SetParent(null);

                rigidbody.isKinematic = false;
                rigidbody.linearVelocity = transform.up * 25f;
            }
        }

        m_Barrels.RemoveAt(i);
    }

    private void SetupBarrels()
    {
        m_Barrels.Clear();

        for (int i = 0; i < m_BarrelTransforms.Length; i++)
        {
            var pooledObject = AmmunitionPool.Current.GetPooledObjectOfType(AmmunitionType.Drumbomb);

            if (pooledObject != null)
            {
                if (pooledObject.TryGetComponent(out Rigidbody rigidbody))
                {
                    rigidbody.isKinematic = true;

                    pooledObject.transform.SetParent(m_BarrelTransforms[i]);
                    pooledObject.transform.position = m_BarrelTransforms[i].position;
                    pooledObject.transform.rotation = m_BarrelTransforms[i].rotation;
                    pooledObject.SetActive(true);

                    m_Barrels.Add(pooledObject);
                }
            }
        }
    }
}
