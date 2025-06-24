using System.Collections;
using UnityEngine;

public class PickupSpawn : MonoBehaviour
{
    private void Start()
    {
        SpawnPickup();
    }

    public void SpawnPickupDelayed(int delay)
    {
        StartCoroutine(SpawnerDelay(delay));
    }

    private IEnumerator SpawnerDelay(int delay)
    {
        yield return new WaitForSeconds(delay);

        SpawnPickup();
    }

    private void SpawnPickup()
    {
        var pooledObject = PickupPool.Current.GetPooledObject();

        if (pooledObject != null)
        {
            pooledObject.transform.position = transform.position;
            pooledObject.transform.rotation = transform.rotation;
            pooledObject.SetActive(true);

            if (pooledObject.TryGetComponent(out Pickup pickup))
            {
                pickup.Spawner = this;
            }
        }
    }
}
