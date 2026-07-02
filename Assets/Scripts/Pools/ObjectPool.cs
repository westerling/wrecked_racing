using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    [SerializeField]
    protected List<PooledObject> m_ObjectsToPool = new();

    private List<GameObject> m_PooledObjects = new();

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

    protected virtual void InstantiateObjects()
    {
        PooledObjects.Clear();
        
        GameObject temp;

        foreach (var objectToPool in m_ObjectsToPool)
        {
            for (var i = 0; i < objectToPool.Amount; i++)
            {
                temp = Instantiate(objectToPool.ObjectToPool, transform);
                temp.SetActive(false);
                PooledObjects.Add(temp);
            }
        }
    }

    public virtual void ReturnObjectToPool(GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);
        objectToReturn.transform.SetParent(transform);
        objectToReturn.transform.localPosition = Vector3.zero;
        objectToReturn.transform.localRotation = Quaternion.identity;
    }
}
