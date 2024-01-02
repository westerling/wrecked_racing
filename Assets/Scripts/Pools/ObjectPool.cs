using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<PooledObject> m_ObjectsToPool;

    private List<GameObject> m_PooledObjects;

    public List<GameObject> PooledObjects
    {
        get => m_PooledObjects;
        set => m_PooledObjects = value;
    }

    private void Awake()
    {
        CreateInstance();
        InstantiateObjects();
    }

    public abstract void CreateInstance();

    public void InstantiateObjects()
    {
        PooledObjects = new List<GameObject>();
        GameObject temp;

        foreach (var objectToPool in m_ObjectsToPool)
        {
            for (int i = 0; i < objectToPool.Amount; i++)
            {
                temp = Instantiate(objectToPool.ObjectToPool, transform);
                temp.SetActive(false);
                PooledObjects.Add(temp);
            }
        }
    }
}
