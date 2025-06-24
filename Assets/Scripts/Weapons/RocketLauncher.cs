using UnityEngine;

public class RocketLauncher : TargetWeapon
{
    [SerializeField]
    private Transform m_BulletOrigin;

    protected override void Fire()
    {
        AddMuzzleFlash();

        var rocket = AmmunitionPool.Current.GetPooledObjectOfType(WeaponType.RocketLauncher);

        if (rocket != null)
        {
            rocket.transform.SetPositionAndRotation(m_BulletOrigin.position, m_BulletOrigin.rotation);
            rocket.SetActive(true);
        }

        if (rocket.TryGetComponent(out HomingMissile homingMissile))
        {
            homingMissile.ActivateMissile();

            if (Physics.Raycast(m_BulletOrigin.position, m_BulletOrigin.forward, out var hit, 25f, LayerMasks.ShootableLayerMask))
            {
                if (hit.transform.gameObject.TryGetComponent(out Car car))
                {
                    homingMissile.SetTarget(car.transform);
                }
            }
        }
    }

    private void AddMuzzleFlash()
    {
        var muzzleFlash = FxPool.Current.GetPooledObjectOfType(ParticleType.MuzzleFlash);

        if (muzzleFlash != null)
        {
            muzzleFlash.transform.SetPositionAndRotation(m_BulletOrigin.position, m_BulletOrigin.rotation);
            muzzleFlash.SetActive(true);
        }
    }
}
