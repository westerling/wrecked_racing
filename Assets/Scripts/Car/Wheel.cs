using System;
using UnityEngine;

public class Wheel : CarComponent
{
    [SerializeField]
    private WheelPlacement m_WheelPlacement;

    [SerializeField]
    private GameObject m_WheelMesh;

    [SerializeField]
    private Transform m_WheelOuterEdge;

    [SerializeField]
    private TrailRenderer m_SkidMark;

    private float m_SteerAngle;
    private float m_GripFactor;

    private bool m_IsGrounded;

    private Vector3 m_LastSteeringDirection;

    private AnimationCurve m_TireGripCurve;

    public WheelPlacement WheelPlacement 
    {
        get => m_WheelPlacement;
    }
    
    public float SteerAngle 
    {
        get => m_SteerAngle;
        set => m_SteerAngle = value;
    }
    
    private void Start()
    {
        SetGripCurve();
    }

    public float CalculateWheelRadius()
    {
        return Vector3.Distance(transform.position, m_WheelOuterEdge.position);
    }

    private void SetGripCurve()
    {
        m_TireGripCurve =
            WheelPlacement == WheelPlacement.FrontLeft ||
            WheelPlacement == WheelPlacement.FrontRight ?
            Car.Stats.FrontTireGrip : Car.Stats.RearTireGrip;
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + SteerAngle, transform.localRotation.z);
        UpdateMesh();
        CheckSkidMark();
    }

    private void CheckSkidMark()
    {
        m_SkidMark.emitting = m_GripFactor < 0.5 && m_IsGrounded;
    }

    private void FixedUpdate()  
    {
        CalculateFriction();
    }

    public void ApplyAccelerationForce(float force, Vector3 direction, Vector3 position)
    {
        if (Physics.Raycast(transform.position, -transform.up, Car.Stats.SuspensionRestDistance))
        {
            Car.Rigidbody.AddForceAtPosition(direction * force, position);
        }
    }

    private void CalculateFriction()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var hit, Car.Stats.SuspensionRestDistance))
        {
            m_IsGrounded = true;

            var tireLocalVelocity = Car.Rigidbody.GetRelativePointVelocity(transform.position).z;
            var steeringDirection = tireLocalVelocity < 0 ? transform.right : -transform.right;
            var tireWorldVelocity = Car.Rigidbody.GetPointVelocity(transform.position);
            var steeringVelocity = Vector3.Dot(steeringDirection, tireWorldVelocity);
           
            m_GripFactor = m_TireGripCurve.Evaluate(steeringVelocity / tireWorldVelocity.magnitude);

            var desiredVelocityChange = -steeringVelocity * m_GripFactor;
            var desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;
            var force = steeringDirection * Car.Stats.TireMass * desiredAcceleration;

            Car.Rigidbody.AddForceAtPosition(force, hit.point);
        }

        m_IsGrounded = false;
    }

    private void UpdateMesh()
    {
        UpdatePosition();
        UpdateRotation();
    }

    private void UpdatePosition()
    {
        m_WheelMesh.transform.position = transform.position;
    }

    private void UpdateRotation()
    {
        var carSpeed = Vector3.Dot(Car.transform.forward, Car.Rigidbody.velocity);
        var distanceTraveled = carSpeed * Time.deltaTime;
        var rotationInRadians = distanceTraveled / 0.33f;
        var rotationInDegrees = rotationInRadians * Mathf.Rad2Deg;
        m_WheelMesh.transform.Rotate(rotationInDegrees, 0, 0);
    }
}
