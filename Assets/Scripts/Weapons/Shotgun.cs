using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField]
    private Transform[] m_BulletOrigins;

    [Header("Sounds")]
    [SerializeField]
    private Sound m_ShootSound;

    protected override void Fire()
    {
        AddMuzzleFlash();
        AddImpact();
        SoundFxManager.Current.PlaySoundClip(m_ShootSound, transform);
    }

    private void AddImpact()
    {

        foreach (var bulletOrigin in m_BulletOrigins)
        {
            if (Physics.Raycast(bulletOrigin.position, bulletOrigin.forward, out var hit, 25f, LayerMasks.ShootableLayerMask))
            {
                AddHitEffect(hit, bulletOrigin.position);

                if (hit.rigidbody != null && hit.transform != ParentCar.transform)
                {
                    ApplyImpactForce(hit.rigidbody, bulletOrigin.forward, 10000f, 1000f);
                }

                if (hit.collider.gameObject.TryGetComponent(out Health health))
                {
                    health.Damage(16f);
                }
            }
        }
    }

    private void AddHitEffect(RaycastHit hit, Vector3 originPosition)
    {
        var hitEffect = FxPool.Current.GetPooledObjectOfType(ParticleType.HitEffect);
        var directionToShooter = (originPosition - hit.point).normalized;
        hitEffect.transform.forward = directionToShooter;
        if (hitEffect != null)
        {
            hitEffect.transform.SetPositionAndRotation(hit.point, Quaternion.LookRotation(directionToShooter));
            hitEffect.SetActive(true);
        }
    }

    private void ApplyImpactForce(Rigidbody rigidbody, Vector3 forwardDirection, float forceAmount, float sideForceAmount)
    {
        var right = Vector3.Cross(Vector3.up, forwardDirection).normalized;
        var sideDirection = Random.Range(-1f, 1f);
        var totalForce = forwardDirection.normalized * forceAmount + sideDirection * sideForceAmount * right;

        rigidbody.AddForce(totalForce, ForceMode.Impulse);
    }

    private void AddMuzzleFlash()
    {

        foreach (var bulletOrigin in m_BulletOrigins)
        {
            var muzzleFlash = FxPool.Current.GetPooledObjectOfType(ParticleType.MuzzleFlash);

            if (muzzleFlash != null)
            {
                muzzleFlash.transform.SetPositionAndRotation(bulletOrigin.position, bulletOrigin.rotation);
                muzzleFlash.SetActive(true);
            }
        }
    }
}
