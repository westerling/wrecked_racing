using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class AiInputManager : InputManager
{
    [SerializeField]
    private Car m_Car;

    [SerializeField]
    private SplineContainer m_Spline;

    private float m_MaxSpeed = 20f;
    private float m_LookAheadDistance = 5f;
    private float m_NearestPoint;
    private float m_SteerSensitivity = 1f;

    protected override void Awake()
    {
        base.Awake();

        m_Car.InputManager = this;
    }

    private void Update()
    {
        if (m_Spline == null)
        {
            return;
        }

        UpdateSplinePosition();
        DriveAlongSpline();
    }

    private void UpdateSplinePosition()
    {
        SplineUtility.GetNearestPoint(
            m_Spline.Spline,
            transform.position,
            out float3 _,
            out float nearestPoint);

        m_NearestPoint = nearestPoint;
    }

    private void DriveAlongSpline()
    {
        var targetPoint = m_NearestPoint + m_LookAheadDistance / m_Spline.CalculateLength();

        if (targetPoint > 1f)
        {
            targetPoint -= 1f;
        }

        var targetPosition = m_Spline.EvaluatePosition(targetPoint);
        var direction = ((Vector3)targetPosition - transform.position).normalized;
        var localDirection = transform.InverseTransformDirection(direction);

        var steer = Mathf.Clamp(localDirection.x * m_SteerSensitivity, -1f, 1f);
        var speed = m_Car.Rigidbody.linearVelocity.magnitude;
        var throttle = speed < m_MaxSpeed ? 1f : 0f;
        var brake = speed > m_MaxSpeed ? 0.5f : 0f;


        Debug.Log(steer + ", " + throttle + ", " + brake);
        SendSteer(steer);
        SendAccelerate(throttle);
        SendBrake(brake);
    }
}
