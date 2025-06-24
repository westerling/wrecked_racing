using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private WeaponType m_WeaponType;

    private PickupSpawn m_Spawner;

    public PickupSpawn Spawner
    {
        get => m_Spawner;
        set => m_Spawner = value;
    }

    void Update()
    {
        transform.Rotate(0f, 64 * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WeaponManager weaponManager))
        {
            if (weaponManager.Weapon != null)
            {
                return;
            }

            weaponManager.AddWeapon(m_WeaponType);
            gameObject.SetActive(false);
            Spawner.SpawnPickupDelayed(5);
        }
    }
}
