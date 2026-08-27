using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class AIInputManager : InputManager
{
    [SerializeField]
    private AICarController m_AICarController;

    [SerializeField]
    private SplineContainer m_Spline;

    private float m_DetectionDistance = 10f;
    private float m_MinFollowDistance = 3f;
    private float m_MaxSpeed = 20f;
    private float m_NearestPoint;
    private float m_SteerSensitivity = 5f;
    private float m_StuckSpeedThreshold = 1f;
    private float m_StuckTime = 1f;
    private float m_ReverseTime = 3f;
    private float m_ReverseThrottle = 1f;
    private float m_StuckTimer;
    private float m_ReverseTimer;

    private bool m_IsReversing;

    private void Update()
    {
        if (m_Spline == null)
        {
            return;
        }

        if (m_IsReversing)
        {
            Reverse();
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

    private void Reverse()
    {
        m_ReverseTimer -= Time.deltaTime;

        SendBrake(1f);
        SendSteer(-1f);

        if (m_ReverseTimer <= 0f)
        {
            m_IsReversing = false;
        }
    }

    private bool TryGetCarAhead(out Rigidbody targetRb, out float distance)
    {
        targetRb = null;
        distance = 0f;

        var origin = transform.position + transform.up * 0.5f;

        if (Physics.SphereCast(
            origin,
            1.2f,
            transform.forward,
            out RaycastHit hit,
            m_DetectionDistance,
            LayerMasks.CarLayerMask))
        {
            if (hit.rigidbody != null &&
                hit.rigidbody != m_AICarController.Rigidbody)
            {
                targetRb = hit.rigidbody;
                distance = hit.distance;
                return true;
            }
        }

        return false;
    }

    private void DriveAlongSpline()
    {
        var speed = m_AICarController.Rigidbody.linearVelocity.magnitude;
        var dynamicLookAhead = Mathf.Lerp(5f, 15f, Mathf.Clamp01(speed / m_MaxSpeed));
        var targetPoint = m_NearestPoint + dynamicLookAhead / m_Spline.CalculateLength();

        if (targetPoint > 1f)
        {
            targetPoint -= 1f;
        }

        var targetPosition = m_Spline.EvaluatePosition(targetPoint);
        var direction = ((Vector3)targetPosition - transform.position).normalized;
        var localDirection = transform.InverseTransformDirection(direction);
        var steer = Mathf.Clamp(localDirection.x * m_SteerSensitivity, -1f, 1f);
        var allowedSpeed = GetCornerSpeed(speed);

        if (TryGetCarAhead(out Rigidbody targetRb, out float distance))
        {
            var targetSpeed = targetRb.linearVelocity.magnitude;

            var followFactor = Mathf.InverseLerp(
                m_MinFollowDistance,
                m_DetectionDistance,
                distance);

            allowedSpeed = Mathf.Min(
                allowedSpeed,
                Mathf.Lerp(targetSpeed, allowedSpeed, followFactor));
        }

        var speedError = allowedSpeed - speed;
        var throttle = Mathf.Clamp01(speedError / allowedSpeed);
        var brake = speedError < 0f ? Mathf.Clamp01(-speedError / allowedSpeed) : 0f;

        SendSteer(steer);
        SendAccelerate(throttle);
        SendBrake(brake);
        UpdateStuckDetection(speed, throttle);
    }

    private float GetCornerSpeed(float speed)
    {
        var lookAhead = Mathf.Lerp(0.01f, 0.05f, Mathf.Clamp01(speed / m_MaxSpeed));
        var p1 = m_NearestPoint;
        var p2 = m_NearestPoint + lookAhead;

        if (p2 > 1f)
        {
            p2 -= 1f;
        }

        var dir1 = m_Spline.EvaluateTangent(p1);
        var dir2 = m_Spline.EvaluateTangent(p2);

        var angle = Vector3.Angle(dir1, dir2);
        var cornerFactor = Mathf.InverseLerp(0f, 60f, angle);

        return Mathf.Lerp(m_MaxSpeed, m_MaxSpeed * 0.4f, cornerFactor);
    }

    private void UpdateStuckDetection(float speed, float throttle)
    {
        if (throttle > 0.5f && speed < m_StuckSpeedThreshold)
        {
            m_StuckTimer += Time.deltaTime;

            if (m_StuckTimer >= m_StuckTime)
            {
                StartReversing();
            }
        }
        else
        {
            m_StuckTimer = 0f;
        }
    }

    private void StartReversing()
    {
        m_IsReversing = true;
        m_ReverseTimer = m_ReverseTime;
        m_StuckTimer = 0f;
    }
}
