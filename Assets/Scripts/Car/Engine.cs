using UnityEngine;

public class Engine : InputComponent
{
    private float m_CurrentAccelerationInput = 0f;
    private float m_CurrentBrakeInput = 0f;

    private RaceStatus m_RaceStatus;

    protected override void AddListeners()
    {
        Car.InputManager.Accelerate += OnAccelerationPerformed;
        Car.InputManager.Brake += OnBrakePerformed;
        RaceManager.Current.RaceStatusChanged += OnRaceStateChanged;
    }

    protected override void RemoveListeners()
    {
        Car.InputManager.Accelerate -= OnAccelerationPerformed;
        Car.InputManager.Brake -= OnBrakePerformed;
        RaceManager.Current.RaceStatusChanged -= OnRaceStateChanged;
    }

    private void Update()
    {
        if (m_RaceStatus == RaceStatus.Race)
        {
            ApplyAcceleration();
        }
    }

    private void ApplyAcceleration()
    {
        var motorTorque = 0f;

        if (m_CurrentAccelerationInput > 0)
        {
            motorTorque = Car.CurrentSpeed < (Car.Stats.TopSpeed * Car.StatusManager.GetModifierAmount(Stat.Speed)) ?
            (m_CurrentAccelerationInput * Car.Stats.MotorTorque.Evaluate(Car.CurrentSpeedRatio) * Car.StatusManager.GetModifierAmount(Stat.Acceleration))
            : 0;
        }

        else if (m_CurrentBrakeInput > 0)
        {
            if (Car.HeadingDirection != CarDirection.Forward)
            {
                motorTorque = -(Car.CurrentSpeed > -(Car.Stats.TopSpeed / 8) ?
                (m_CurrentBrakeInput * Car.Stats.MotorTorque.Evaluate(Car.CurrentSpeedRatio))
                : 0);
            }       
        }

        Car.Transmission.MotorTorque = motorTorque;
    }

    private void OnRaceStateChanged(RaceStatus obj)
    {
        m_RaceStatus = obj;
    }

    private void OnAccelerationPerformed(float obj)
    {
        m_CurrentAccelerationInput = obj;
    }

    private void OnBrakePerformed(float obj)
    {
        m_CurrentBrakeInput = obj;
    }
}
