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
            for (int i = 0; i < pooledSoundsObject.Amount; i++)
            {
                var clipGO = new GameObject($"AudioSource_{pooledSoundsObject.ObjectToPool.name}");
                clipGO.SetActive(false);

                var source = SetupAudioSource(clipGO);
                var soundFx = clipGO.AddComponent<SoundFx>();
                soundFx.SoundFxType = pooledSoundsObject.SoundFxType;
                soundFx.AudioClip = pooledSoundsObject.ObjectToPool;

                source.clip = soundFx.AudioClip;

                m_ObjectsToPool.Add(new PooledObject
                {
                    ObjectToPool = clipGO,
                    Amount = 1
                });
            }
        }
    }

    private AudioSource SetupAudioSource(GameObject clipGO)
    {
        var source = clipGO.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 5f;
        source.maxDistance = 50f;

        return source;
    }

    public GameObject GetPooledObjectOfType(SoundFxType soundFxType)
    {
        var pooledObjects = PooledObjects
            .Where(x => !x.activeInHierarchy && x.GetComponent<SoundFx>().SoundFxType == soundFxType)
            .ToList();

        if (pooledObjects.Count > 0)
            return pooledObjects[Random.Range(0, pooledObjects.Count)];

        Debug.LogWarning($"No available SoundFxType of {soundFxType}");
        return null;
    }
}
