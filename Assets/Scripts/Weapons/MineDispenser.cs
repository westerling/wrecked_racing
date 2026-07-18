using UnityEngine;

public class MineDispenser : Weapon
{
    [SerializeField]
    private GameObject m_TopMine;


    private void Awake()
    {
        WeaponType = WeaponType.Mine;
        m_TopMine.SetActive(true);
    }

    protected override void Fire()
    {
        DropMine();
    }

    private void DropMine()
    {
        var pooledObject = AmmunitionPool.Current.GetPooledObjectOfType(AmmunitionType.Mine);

        if (pooledObject != null)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 5f, LayerMasks.DriveableLayerMask))
            {
                pooledObject.transform.position = hit.point;
                pooledObject.transform.rotation = transform.rotation;
                pooledObject.SetActive(true);

                if (pooledObject.TryGetComponent(out Mine mine))
                {
                    mine.PlaceMine();

                    if (Ammunition == 1)
                    {
                        m_TopMine.SetActive(false);
                    }
                }
            }
        }
    }
}
