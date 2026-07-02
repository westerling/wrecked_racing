using System.Linq;
using UnityEngine;

public class TrailPool : ObjectPool
{
    public static TrailPool Current;
    
    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObjectOfType(SurfaceType surfaceType)
    {
        var pooledObject = PooledObjects.FirstOrDefault(x => !(x.activeInHierarchy) && x.GetComponent<PooledSkidTrail>().SurfaceType == surfaceType);

        if (pooledObject == null)
        {
            Debug.LogError("Object does not exist");

            return null;
        }

        return pooledObject;
    }
}
