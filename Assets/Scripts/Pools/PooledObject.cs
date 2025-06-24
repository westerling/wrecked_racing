using System;
using UnityEngine;

[Serializable]
public class PooledObject
{
    [SerializeField]
    private GameObject m_ObjectToPool;

    [SerializeField]
    private int m_Amount;

    public GameObject ObjectToPool
    {
        get => m_ObjectToPool;
    }

    public int Amount
    {
        get => m_Amount;
    }
}
