using System;
using static UnityEngine.UI.Image;
using UnityEngine;

public class MineDispenser : Weapon
{
    private void Awake()
    {
        WeaponType = WeaponType.Mine;
    }

    protected override void Fire()
    {
        DropMine();
    }

    private void DropMine()
    {
        var pooledObject = AmmunitionPool.Current.GetPooledObjectOfType(WeaponType.Mine);

        if (pooledObject != null)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 5f, LayerMasks.Driveable))
            {
                pooledObject.transform.position = hit.point;
                pooledObject.transform.rotation = transform.rotation;
                pooledObject.SetActive(true);

                if (pooledObject.TryGetComponent(out Mine mine))
                {
                    mine.PlaceMine();
                }
            }
        }
    }
}
