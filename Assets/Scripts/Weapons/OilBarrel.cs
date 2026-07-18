using UnityEngine;

public class OilBarrel : Weapon
{
    protected override void Fire()
    {
        AddOilSlick();
    }

    private void AddOilSlick()
    {
        var pooledObject = AmmunitionPool.Current.GetPooledObjectOfType(AmmunitionType.Oil);

        if (pooledObject != null)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out var hit, 5f, LayerMasks.DriveableLayerMask))
            {
                pooledObject.transform.position = hit.point;
                pooledObject.transform.rotation = transform.rotation;
                var euler = pooledObject.transform.eulerAngles;
                euler.y = Random.Range(0f, 360f);
                pooledObject.transform.eulerAngles = euler;

                pooledObject.SetActive(true);
            }
        }
    }
}
