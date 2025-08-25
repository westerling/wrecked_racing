using System.Linq;
using UnityEngine;

public class AmmunitionPool : ObjectPool
{
    public static AmmunitionPool Current;

    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObjectOfType(AmmunitionType ammunitionType)
    {
        return PooledObjects.First(x => !(x.activeInHierarchy) && x.GetComponent<Ammunition>().AmmunitionType == ammunitionType);
    }
}
