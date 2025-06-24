using UnityEngine;

public class Bullet : Ammunition
{
    private float m_LifeTime = 0;
    private Transform m_ParentTransform;

    public Transform ParentTransform
    {
        get => m_ParentTransform;
        set => m_ParentTransform = value;
    }

    private void Awake()
    {
        WeaponType = WeaponType.Rifle;
    }

    private void OnEnable()
    {
        m_LifeTime = 0;
    }

    private void Update()
    {
        m_LifeTime += Time.deltaTime;

        if (m_LifeTime > 1)
        {
            Deactivate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == ParentTransform)
        {
            return;
        }

        var layerMask = 1 << other.gameObject.layer;

        if ((LayerMasks.ShootableLayerMask & layerMask) != 0)
        {
            Debug.Log(other.gameObject);
            Deactivate();
        }
    }
}
