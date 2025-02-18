public class Brakes : InputComponent
{
    private float m_CurrentBrakeInput = 0f;

    private RaceStatus m_RaceStatus;

    private void Update()
    {
        ApplyBrakes();
    }

    private void ApplyBrakes()
    {
        var brakeForce = 0f;

        if (m_RaceStatus == RaceStatus.Countdown)
        {
            brakeForce = 1000f;
        }
        else
        {
            brakeForce = Car.HeadingDirection == CarDirection.Forward
            ? m_CurrentBrakeInput * Car.Stats.BrakeStrength.Evaluate(Car.CurrentSpeedRatio) / Car.Wheels.Length
            : 0f;
        }

        foreach (var wheel in Car.Wheels)
        {
            wheel.BrakeTorque = brakeForce;
        }
    }

    private void OnBrakePerformed(float obj)
    {
        m_CurrentBrakeInput = obj;
    }

    private void OnRaceStateChanged(RaceStatus obj)
    {
        m_RaceStatus = obj;
    }

    protected override void AddListeners()
    {
        Car.InputManager.Brake += OnBrakePerformed;
        RaceManager.Current.OnRaceStatus += OnRaceStateChanged;
    }

    protected override void RemoveListeners()
    {
        Car.InputManager.Brake -= OnBrakePerformed;
        RaceManager.Current.OnRaceStatus -= OnRaceStateChanged;
    }
}
