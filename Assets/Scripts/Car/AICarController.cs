using System.Collections.Generic;
using UnityEngine;

public class AICarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private List<SimpleWheel> m_DriveWheels = new List<SimpleWheel>();

    [SerializeField]
    private List<SimpleWheel> m_SteeringWheels = new List<SimpleWheel>();

    [SerializeField]
    private float m_TopSpeed = 15f;

    [SerializeField]
    private AnimationCurve m_MotorTorque;

    [SerializeField]
    private AnimationCurve m_BrakeStrength;

    [SerializeField]
    private AnimationCurve m_TurningCurve;

    [SerializeField]
    private InputManager m_InputManager;

    private int m_NumberOfWheels;

    private float m_CurrentSpeedRatio = 0f;
    private float m_CurrentSpeed = 0f;
    private float m_brakeForce;
    private float m_CurrentAccelerationInput = 0f;
    private float m_CurrentBrakeInput = 0f;
    private float m_CurrentSteerInput = 0f;

    private CarDirection m_HeadingDirection = CarDirection.Stationary;

    public Rigidbody Rigidbody
    {
        get => m_Rigidbody;
    }

    private void Awake()
    {
        m_NumberOfWheels = m_DriveWheels.Count + m_SteeringWheels.Count;
    }

    private void Start()
    {
        AddListeners();
    }

    private void Update()
    {
        ApplyAcceleration();
        ApplyBrakes();
        ApplySteering();
    }

    private void FixedUpdate()
    {
        CalculateCarVelocity();
        SetCarDirection();
    }

    private void AddListeners()
    {
        m_InputManager.Brake += OnBrakePerformed;
        m_InputManager.Accelerate += OnAccelerationPerformed;
        m_InputManager.Steer += OnSteerPerformed;
    }

    private void RemoveListeners()
    {
        m_InputManager.Brake -= OnBrakePerformed;
        m_InputManager.Steer -= OnSteerPerformed;
        m_InputManager.Accelerate -= OnAccelerationPerformed;
    }

    private void ApplyAcceleration()
    {
        var torque = 0f;

        if (m_CurrentAccelerationInput > 0)
        {
            torque = m_CurrentSpeed < m_TopSpeed
                ? m_CurrentAccelerationInput * m_MotorTorque.Evaluate(m_CurrentSpeedRatio)
                : 0;
        }

        else if (m_CurrentBrakeInput > 0)
        {
            if (m_HeadingDirection != CarDirection.Forward)
            {
                torque = -(m_CurrentSpeed > -(m_TopSpeed / 8) ?
                (m_CurrentBrakeInput * m_MotorTorque.Evaluate(m_CurrentSpeedRatio))
                : 0);
            }
        }

        var torquePerWheel = torque / m_DriveWheels.Count;

        foreach (var driveWheel in m_DriveWheels)
        {
            driveWheel.MotorTorque = torquePerWheel;
        }
    }

    private void ApplyBrakes()
    {
        if (m_CurrentBrakeInput == 0 && m_CurrentAccelerationInput == 0 && m_CurrentSpeed < 5f)
        {
            m_brakeForce = 500f;
        }
        else
        {
            m_brakeForce = m_HeadingDirection == CarDirection.Forward
                        ? m_CurrentBrakeInput * m_BrakeStrength.Evaluate(m_CurrentSpeedRatio) / m_NumberOfWheels
                        : 0f;
        }

        foreach (var wheel in m_DriveWheels)
        {
            wheel.BrakeTorque = m_brakeForce;
        }

        foreach (var wheel in m_SteeringWheels)
        {
            wheel.BrakeTorque = m_brakeForce;
        }
    }

    private void ApplySteering()
    {
        foreach (var wheel in m_SteeringWheels)
        {
            var steerAngle = m_CurrentSteerInput * m_TurningCurve.Evaluate(m_CurrentSpeedRatio);
            wheel.SteerAngle = steerAngle;
        }
    }

    private void SetCarDirection()
    {
        if (m_CurrentSpeed > 1)
        {
            m_HeadingDirection = CarDirection.Forward;
        }
        else if (m_CurrentSpeed < -1)
        {
            m_HeadingDirection = CarDirection.Backward;
        }
        else
        {
            m_HeadingDirection = CarDirection.Stationary;
        }
    }

    private void CalculateCarVelocity()
    {
        m_CurrentSpeed = transform.InverseTransformDirection(Rigidbody.linearVelocity).z;
        m_CurrentSpeedRatio = m_CurrentSpeed / m_TopSpeed;
    }

    private void OnBrakePerformed(float obj)
    {
        m_CurrentBrakeInput = obj;
    }

    private void OnAccelerationPerformed(float obj)
    {
        m_CurrentAccelerationInput = obj;
    }

    private void OnSteerPerformed(float obj)
    {
        m_CurrentSteerInput = obj;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
