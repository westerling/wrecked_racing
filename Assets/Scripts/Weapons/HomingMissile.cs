using UnityEngine;

public class HomingMissile : Missile
{
    [Header("MOVEMENT")]
    [SerializeField] private float m_RotateSpeed = 95;

    [Header("PREDICTION")]
    [SerializeField] private float m_MaxDistancePredict = 100;
    [SerializeField] private float m_MinDistancePredict = 5;
    [SerializeField] private float m_MaxTimePrediction = 5;


    [Header("DEVIATION")]
    [SerializeField] private float m_DeviationAmount = 50;
    [SerializeField] private float m_DeviationSpeed = 2;

    private Transform m_Target;
    private Rigidbody m_TargetRigidBody;

    private Vector3 m_StandardPrediction;
    private Vector3 m_DeviatedPrediction;

    public Rigidbody TargetRigidBody
    {
        get => m_TargetRigidBody;
        set => m_TargetRigidBody = value;
    }

    private void Awake()
    {
        AmmunitionType = AmmunitionType.HomingMissile;
    }

    protected override void UpdatePosition()
    {
        if (m_Target == null || m_TargetRigidBody == null)
        {
            return;
        }

        RigidBody.linearVelocity = transform.forward * Speed;

        var leadTimePercentage = Mathf.InverseLerp(m_MinDistancePredict, m_MaxDistancePredict, Vector3.Distance(transform.position, m_Target.position));

        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);
        RotateRocket();
    }

    private void LateUpdate()
    {
        if (m_Target == null || m_TargetRigidBody == null)
        {
            transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
        }
    }

    public void ActivateMissile(Transform origin, Transform target, float speed)
    {
        Speed = speed;

        transform.SetPositionAndRotation(origin.position, origin.rotation);
        transform.parent = null;

        RigidBody.linearVelocity = Vector3.zero;
        RigidBody.angularVelocity = Vector3.zero;

        m_Target = target;
        m_TargetRigidBody = target.GetComponent<Rigidbody>();

        AddPooledObject();
        StartCoroutine(ActivateAfterDelay());
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, m_MaxTimePrediction, leadTimePercentage);

        m_StandardPrediction = TargetRigidBody.position + TargetRigidBody.linearVelocity * predictionTime;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * m_DeviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * m_DeviationAmount * leadTimePercentage;

        m_DeviatedPrediction = m_StandardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = m_DeviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        
        RigidBody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, m_RotateSpeed * Time.deltaTime));
    }
}
