using UnityEngine;

public class RocketLauncher : TargetWeapon
{
    [SerializeField]
    private Transform m_BulletOrigin;

    [Header("Sounds")]
    [SerializeField]
    private Sound m_ExplosionSound;

    protected override void Fire()
    {        
        if (Physics.Raycast(m_BulletOrigin.position, m_BulletOrigin.forward, out var hit, 25f, LayerMasks.ShootableLayerMask))
        {
            if (hit.transform.gameObject.TryGetComponent(out Car car))
            {
                ActivateHomingMissile(car.transform);
            }
            else
            {
                ActivateDummyMissile();
            }
        }
        else
        {
            ActivateDummyMissile();
        }

        SoundFxManager.Current.PlaySoundClip(m_ExplosionSound, transform);
    }

    private void ActivateHomingMissile(Transform target)
    {
        AddMuzzleFlash();
        var rocket = GetGameObjectFromPool(AmmunitionType.HomingMissile);

        if (rocket.TryGetComponent(out HomingMissile homingMissile))
        {
            homingMissile.ActivateMissile(m_BulletOrigin, target, ParentCar.Stats.TopSpeed);
        }
    }

    private void ActivateDummyMissile()
    {
        AddMuzzleFlash();
        var rocket = GetGameObjectFromPool(AmmunitionType.DummyMissile);

        if (rocket.TryGetComponent(out DummyMissile dummyMissile))
        {
            dummyMissile.ActivateMissile(m_BulletOrigin, ParentCar.Stats.TopSpeed);
        }
    }

    private GameObject GetGameObjectFromPool(AmmunitionType ammunitionType)
    {
        var rocket = AmmunitionPool.Current.GetPooledObjectOfType(ammunitionType);

        if (rocket != null)
        {
            rocket.transform.SetPositionAndRotation(m_BulletOrigin.position, m_BulletOrigin.rotation);
            rocket.SetActive(true);

            return rocket;
        }

        return null;
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
