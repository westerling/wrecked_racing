using System.Linq;
using UnityEngine;

public class WeaponPool : ObjectPool
{
    public static WeaponPool Current;

    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObjectOfType(WeaponType weaponType)
    {
        var pooledObject = PooledObjects.FirstOrDefault(x => !(x.activeInHierarchy) && x.GetComponent<Weapon>().WeaponType == weaponType);

        if (pooledObject == null)
        {
            Debug.LogError("Object does not exist");

            return null;
        }

        return pooledObject;
    }
}
