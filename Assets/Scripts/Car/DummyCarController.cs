using System.Collections.Generic;
using UnityEngine;

public class DummyCarController : InputComponent
{
    [SerializeField]
    private List<Wheel> m_SteeringWheels = new List<Wheel>();

    [SerializeField]
    private List<Wheel> m_DriveWheels = new List<Wheel>();

    private float m_CurrentAccelerationInput;
    private float m_CurrentBrakeInput;
    private float m_CurrentSteerInput;

    protected override void AddListeners()
    {
        Car.InputManager.Accelerate += OnAccelerationPerformed;
        Car.InputManager.Brake += OnBrakePerformed;
        Car.InputManager.Steer += OnSteerPerformed;
    }

    private void Update()
    {
        foreach (var wheel in m_SteeringWheels)
        {
            wheel.SteerAngle = m_CurrentSteerInput;
            wheel.BrakeTorque = m_CurrentBrakeInput;
        }

        foreach (var wheel in m_DriveWheels)
        {
            wheel.MotorTorque = m_CurrentAccelerationInput;
            wheel.BrakeTorque = m_CurrentBrakeInput;
        }
    }

    private void OnSteerPerformed(float obj)
    {
        m_CurrentSteerInput = obj;
    }

    private void OnAccelerationPerformed(float obj)
    {
        m_CurrentAccelerationInput = obj;
    }

    private void OnBrakePerformed(float obj)
    {
        m_CurrentBrakeInput = obj;
    }

    protected override void RemoveListeners()
    {
        Car.InputManager.Accelerate -= OnAccelerationPerformed;
        Car.InputManager.Brake -= OnBrakePerformed;
    }
}
