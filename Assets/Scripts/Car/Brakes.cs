using System;

public class Brakes : InputComponent
{
    private float m_CurrentBrakeInput = 0f;
    private float m_brakeForce;
    private bool m_CarActive = false;

    private RaceStatus m_RaceStatus;

    protected override void Start()
    {
        base.Start();

        Car.CarStatus += OnCarStatus;
    }

    private void OnCarStatus(Car car, bool status)
    {
        m_CarActive|= status;
    }

    private void Update()
    {
        ApplyBrakes();
    }

    private void ApplyBrakes()
    {
        if (m_RaceStatus == RaceStatus.Race && m_CarActive)
        {
            m_brakeForce = Car.HeadingDirection == CarDirection.Forward
            ? m_CurrentBrakeInput * Car.Stats.BrakeStrength.Evaluate(Car.CurrentSpeedRatio) / Car.Wheels.Length
            : 0f;
        }
        else
        {
            m_brakeForce = 1000f;
        }

        foreach (var wheel in Car.Wheels)
        {
            wheel.BrakeTorque = m_brakeForce;
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

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Car.CarStatus -= OnCarStatus;
    }
}
