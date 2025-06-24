using System.Collections;
using UnityEngine;

public class HomingMissile : Ammunition
{
    [Header("REFERENCES")]
    [SerializeField]
    private Rigidbody m_RigidBody;

    [Header("Blast")]
    [SerializeField]
    private float m_Radius = 1.0f;
    [SerializeField]
    private float m_Power = 1.0f;

    [Header("MOVEMENT")]
    [SerializeField] private float m_Speed = 15;
    [SerializeField] private float m_RotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;


    [Header("DEVIATION")]
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 2;

    private Transform m_Target;
    private Rigidbody m_TargetRigidBody;

    private Vector3 _standardPrediction, _deviatedPrediction;

    private bool m_Active = false;

    public Rigidbody TargetRigidBody
    {
        get => m_TargetRigidBody;
        set => m_TargetRigidBody = value;
    }

    private void Awake()
    {
        WeaponType = WeaponType.RocketLauncher;
    }

    private void FixedUpdate()
    {
        if (m_Target == null)
        {
            UseDummyMissile();
            return;
        }

        UseHomingMissile();
    }

    private void LateUpdate()
    {
        if (m_Target == null)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }
    }

    public void ActivateMissile()
    {
        StartCoroutine(SetMineSafety());
    }

    private IEnumerator SetMineSafety()
    {
        m_Active = false;
        yield return new WaitForSeconds(1f);
        m_Active = true;
    }

    public void SetTarget(Transform target)
    {
        m_Target = target;
        m_TargetRigidBody = target.GetComponent<Rigidbody>();
    }

    private void UseDummyMissile()
    {
        m_RigidBody.linearVelocity = transform.forward * m_Speed;

        if (Physics.Raycast(transform.position, Vector3.down, out var hit, 10f, LayerMasks.Driveable))
        {
            var currentAltitude = transform.position.y - hit.point.y;
            var altitudeError = 2f - currentAltitude;
            var yAdjustment = altitudeError;
            var correctedVelocity = m_RigidBody.linearVelocity;
            correctedVelocity.y = yAdjustment;
            
            m_RigidBody.linearVelocity = correctedVelocity;
        }
    }

    private void UseHomingMissile()
    {
        m_RigidBody.linearVelocity = transform.forward * m_Speed;

        var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, m_Target.position));

        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);
        RotateRocket();
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = TargetRigidBody.position + TargetRigidBody.linearVelocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        m_RigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, m_RotateSpeed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!m_Active)
        {
            return;
        }

        var explosionPos = transform.position;
        var colliders = Physics.OverlapSphere(explosionPos, m_Radius);

        var explosion = FxPool.Current.GetPooledObjectOfType(ParticleType.Explosion_m);

        if (explosion != null)
        {
            explosion.transform.position = explosionPos;
            explosion.SetActive(true);
        }

        foreach (var hit in colliders)
        {
            var rigidbody = hit.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(m_Power, explosionPos, m_Radius, 300f);
            }
        }

        Deactivate();
    }
}
