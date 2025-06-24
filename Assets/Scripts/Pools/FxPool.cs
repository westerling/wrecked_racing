using System.Linq;
using UnityEngine;

public class FxPool : ObjectPool
{
    public static FxPool Current;

    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObjectOfType(ParticleType particleType)
    {
        var pooledObject = PooledObjects.FirstOrDefault(x => !(x.activeInHierarchy) && x.GetComponent<SpecialEffect>().ParticleType == particleType);

        if (pooledObject == null)
        {
            Debug.LogError("Object does not exist");

            return null;
        }

        return pooledObject;
    }
}
