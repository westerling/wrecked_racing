using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Engine : CarComponent
{
    private float m_CurrentAccelerationInput;

    private List<Wheel> m_DriveWheels = new List<Wheel>();

    private void Start()
    {
        AddListeners();
        SetDriveWheels();
    }

    private void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out var hit, Car.Stats.SuspensionRestDistance))
        {
            var carSpeed = Vector3.Dot(Car.transform.forward, Car.Rigidbody.velocity);

            if (carSpeed > Car.Stats.TopSpeed + Car.StatusManager.SpeedModifier)
            {
                return;
            }

            var normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / Car.Stats.TopSpeed);
            var availableTorque = Car.Stats.Torque.Evaluate(normalizedSpeed) * Car.Stats.HorsePower * m_CurrentAccelerationInput;

            foreach (var wheel in m_DriveWheels)
            {
                wheel.ApplyAccelerationForce(availableTorque, transform.forward, hit.point);
            }
        }
    }

    private void AddListeners()
    {
        Car.InputManager.Accelerate += AccelerationPerformed;
    }

    private void RemoveListeners()
    {
        Car.InputManager.Accelerate -= AccelerationPerformed;
    }

    private void AccelerationPerformed(float obj)
    {
        m_CurrentAccelerationInput = obj;
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

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
