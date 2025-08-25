using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundFxPool : ObjectPool
{
    public static SoundFxPool Current;

    [SerializeField]
    private List<PooledSoundsObject> m_FxObjectsToPool;

    public override void CreateInstance()
    {
        Current = this;
    }

    protected override void InstantiateObjects()
    {
        CreateGameObjects();
        base.InstantiateObjects();
    }

    private void CreateGameObjects()
    {
        foreach (var pooledSoundsObject in m_FxObjectsToPool)
        {
            var clipGO = new GameObject("AudioSource_" + pooledSoundsObject.ObjectToPool.name);
            var source = clipGO.AddComponent<SoundFx>();
            
            source.SoundFxType = pooledSoundsObject.SoundFxType;
            source.AudioClip = pooledSoundsObject.ObjectToPool;


            m_ObjectsToPool.Add(new PooledObject
            {
                ObjectToPool = clipGO,
                Amount = pooledSoundsObject.Amount
            });
        }
    }

    public GameObject GetPooledObjectOfType(SoundFxType soundFxType)
    {
        var pooledObjects = PooledObjects.Where(x => !(x.activeInHierarchy) && x.GetComponent<SoundFx>().SoundFxType == soundFxType).ToList();

        if (pooledObjects == null || pooledObjects.Count() == 0)
        {
            Debug.LogError("Object does not exist");

            return null;
        }

        var randomNumber = Random.Range(0, pooledObjects.Count());
        return pooledObjects[randomNumber];
    }
}
