using UnityEngine;

public class Rifle : TargetWeapon
{
    [SerializeField]
    private Transform m_BulletOrigin;

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
        if (Physics.Raycast(m_BulletOrigin.position, m_BulletOrigin.forward, out var hit, 25f, LayerMasks.ShootableLayerMask))
        {
            AddHitEffect(hit);

            if (hit.rigidbody != null && hit.transform != ParentCar.transform)
            {
                ApplyImpactForce(hit.rigidbody, m_BulletOrigin.forward, 10000f, 1000f);
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

    private void AddHitEffect(RaycastHit hit)
    {
        var hitEffect = FxPool.Current.GetPooledObjectOfType(ParticleType.HitEffect);
        var directionToShooter = (m_BulletOrigin.position - hit.point).normalized;
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
}
