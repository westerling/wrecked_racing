using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brakes : CarComponent
{
    private float m_CurrentBrakeInput;

    private List<Wheel> m_DriveWheels = new List<Wheel>();

    private void Start()
    {
        AddListeners();
        SetDriveWheels();
    }

    private void FixedUpdate()
    {
        if (m_CurrentBrakeInput <= 0)
        {
            return;
        }

        if (Physics.Raycast(transform.position, -transform.up, out var hit, Car.Stats.SuspensionRestDistance))
        {
            var carSpeed = Vector3.Dot(Car.transform.forward, Car.Rigidbody.velocity);
            
            if (carSpeed > 1)
            {
                ApplyBrakes(hit.point);
                return;
            }

            Reverse(hit.point);
        }
    }

    private void Reverse(Vector3 hit)
    {
        var carSpeed = Vector3.Dot(-Car.transform.forward, Car.Rigidbody.velocity);
        var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / (Car.Stats.TopSpeed / 10));
        var availableTorque = Car.Stats.Torque.Evaluate(normalizedSpeed) * Car.Stats.HorsePower * m_CurrentBrakeInput;

        foreach (var wheel in m_DriveWheels)
        {
            wheel.ApplyAccelerationForce(availableTorque, -transform.forward, hit);
        }
    }

    private void ApplyBrakes(Vector3 hit)
    {
        var availableTorque = Car.Stats.Brakes * m_CurrentBrakeInput;

        foreach (var wheel in Car.Wheels)
        {
            wheel.ApplyAccelerationForce(availableTorque, -transform.forward, hit);
        }
    }

    private void SetDriveWheels()
    {
        switch (Car.Stats.DriveTrain)
        {
            case Drivetrain.FWD:
                m_DriveWheels.AddRange(Car.Wheels.Where(
                    x => x.WheelPlacement == WheelPlacement.FrontLeft ||
                    x.WheelPlacement == WheelPlacement.FrontRight));
                break;
            case Drivetrain.RWD:
                m_DriveWheels.AddRange(Car.Wheels.Where(
                    x => x.WheelPlacement == WheelPlacement.FrontLeft ||
                    x.WheelPlacement == WheelPlacement.FrontRight));
                break;
            case Drivetrain.AWD:
                m_DriveWheels.AddRange(Car.Wheels);
                break;
        }
    }

    private void AddListeners()
    {
        Car.InputManager.Brake += BrakePerformed;
    }

    private void RemoveListeners()
    {
        Car.InputManager.Brake -= BrakePerformed;
    }

    private void BrakePerformed(float obj)
    {
        m_CurrentBrakeInput = obj;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
