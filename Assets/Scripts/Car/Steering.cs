using System.Collections.Generic;
using System.Linq;

public class Steering : InputComponent
{
    private List<Wheel> m_SteeringWheels = new List<Wheel>();

    private float m_SteerInput = 0f;

    protected override void Awake()
    {
        base.Awake();

        SetSteeringWheels();
    }

    private void SetSteeringWheels()
    {
        m_SteeringWheels.AddRange(Car.Wheels.Where(
            x => x.WheelPlacement == WheelPlacement.FrontLeft ||
            x.WheelPlacement == WheelPlacement.FrontRight));
    }

    private void Update()
    {
        if (!m_SteeringWheels.Any())
        {
            SetSteeringWheels();
            return;
        }

        foreach (var wheel in m_SteeringWheels)
        {
            var steerAngle = m_SteerInput * Car.Stats.TurningCurve.Evaluate(Car.CurrentSpeedRatio);
            wheel.SteerAngle = steerAngle;
        }
    }

    protected override void AddListeners()
    {
        Car.InputManager.Steer += OnSteerPerformed;
    }

    protected override void RemoveListeners()
    {
        Car.InputManager.Steer -= OnSteerPerformed;
    }

    private void OnSteerPerformed(float obj)
    {
        m_SteerInput = obj;
    }
}
