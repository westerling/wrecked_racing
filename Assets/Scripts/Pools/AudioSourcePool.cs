using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : ObjectPool
{
    public static AudioSourcePool Current;

    public override void CreateInstance()
    {
        Current = this;
    }

    public GameObject GetPooledObject()
    {
        var tempList = new List<GameObject>();

        for (var i = 0; i < PooledObjects.Count; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                tempList.Add(PooledObjects[i]);
            }
        }

        if (tempList.Count > 0)
        {
            var randomNumber = Random.Range(0, tempList.Count);
            return tempList[randomNumber];
        }

        return null;
    }
}
