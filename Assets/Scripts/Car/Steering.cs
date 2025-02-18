public class Steering : InputComponent
{
    private float m_SteerInput = 0f;

    private void Update()
    {
        foreach (var wheel in Car.SteeringWheels)
        {
            var steeringAngle = m_SteerInput * Car.Stats.TurningCurve.Evaluate(Car.CurrentSpeedRatio);
            wheel.SteerAngle = steeringAngle;
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
