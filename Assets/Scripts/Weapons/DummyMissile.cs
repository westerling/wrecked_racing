using UnityEngine;

public class DummyMissile : Missile
{
    private Vector3 m_InitialDirection;

    private void Awake()
    {
        AmmunitionType = AmmunitionType.DummyMissile;
    }

    public void ActivateMissile(Transform origin, float startSpeed, float topSpeed)
    {
        TopSpeed = topSpeed;
        AccelerationTimer = 0f;
        Speed = startSpeed + 5f;

        transform.SetPositionAndRotation(origin.position, origin.rotation);
        transform.parent = null;
        m_InitialDirection = origin.forward.normalized;
        RigidBody.linearVelocity = Vector3.zero;
        RigidBody.angularVelocity = Vector3.zero;

        SoundFxManager.Current.PlaySoundClip(TrailSound, transform, () => !Exploded);

        AddPooledObject();
        StartCoroutine(ActivateAfterDelay());
    }

    protected override void UpdatePosition()
    {
        AccelerationTimer += Time.deltaTime;

        var accelerationPercentage =
        Mathf.Clamp01(AccelerationTimer / ACCELERATION_TIME);

        Speed = Mathf.Lerp(Speed, TopSpeed, accelerationPercentage);

        RigidBody.linearVelocity = m_InitialDirection * Speed;

        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10f, LayerMasks.ExplosionLayerMask))
        {
            var currentAltitude = transform.position.y - hit.point.y;
            var altitudeError = 2f - currentAltitude;
            var yAdjustment = altitudeError;
            var correctedVelocity = RigidBody.linearVelocity;
            correctedVelocity.y = yAdjustment;

            RigidBody.linearVelocity = correctedVelocity;
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
    }
}
