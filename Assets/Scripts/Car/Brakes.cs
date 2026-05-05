public class Brakes : InputComponent
{
    private float m_CurrentAccelerationInput = 0f;
    private float m_CurrentBrakeInput = 0f;
    private float m_brakeForce;

    private CarStatus m_CarStatus;
    private RaceStatus m_RaceStatus;

    protected override void Awake()
    {
        base.Awake();

        Car.CarStatusChanged += OnCarStatus;
    }

    private void OnCarStatus(Car car, CarStatus carStatus)
    {
        m_CarStatus = carStatus;
    }

    private void Update()
    {
        ApplyBrakes();
    }

    private void ApplyBrakes()
    {
        if ((m_RaceStatus == RaceStatus.Race && m_CarStatus == CarStatus.Active) || Car.IsAi)
        {
            if (m_CurrentBrakeInput == 0 && m_CurrentAccelerationInput == 0 && Car.CurrentSpeed < 5f)
            {
                m_brakeForce = 500f;
            }
            else
            {
                m_brakeForce = Car.HeadingDirection == CarDirection.Forward
                            ? m_CurrentBrakeInput * Car.Stats.BrakeStrength.Evaluate(Car.CurrentSpeedRatio) / Car.Wheels.Count
                            : 0f;
            }
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

    private void OnAccelerationPerformed(float obj)
    {
        m_CurrentAccelerationInput = obj;
    }

    private void OnRaceStateChanged(RaceStatus obj)
    {
        m_RaceStatus = obj;
    }

    protected override void AddListeners()
    {
        Car.InputManager.Brake += OnBrakePerformed;
        Car.InputManager.Accelerate += OnAccelerationPerformed;
        RaceManager.Current.RaceStatusChanged += OnRaceStateChanged;
    }

    protected override void RemoveListeners()
    {
        Car.InputManager.Brake -= OnBrakePerformed;
        Car.InputManager.Accelerate -= OnAccelerationPerformed;
        RaceManager.Current.RaceStatusChanged -= OnRaceStateChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        Car.CarStatusChanged -= OnCarStatus;
    }
}
