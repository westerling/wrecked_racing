using UnityEngine;

public class Engine : InputComponent
{
    [SerializeField]
    private GameObject m_Flames;

    [Header("Sounds")]
    [SerializeField]
    private Sound m_ExplosionSound;

    private float m_CurrentAccelerationInput = 0f;
    private float m_CurrentBrakeInput = 0f;

    private RaceStatus m_RaceStatus;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        m_Flames.SetActive(false);
    }

    protected override void AddListeners()
    {
        Car.Health.CarHealthChanged += OnCarHealthChanged;
        Car.InputManager.Accelerate += OnAccelerationPerformed;
        Car.InputManager.Brake += OnBrakePerformed;
        RaceManager.Current.RaceStatusChanged += OnRaceStateChanged;
    }

    private void OnCarHealthChanged(float healthPoints, float healthRatio)
    {
        if (healthPoints <= 0)
        {
            SoundFxManager.Current.PlaySoundClip(m_ExplosionSound, transform);
        }

        if (healthRatio < 0.25f)
        {
            if (!m_Flames.activeSelf)
            {
                m_Flames.SetActive(true);
            }
        }
    }

    protected override void RemoveListeners()
    {
        Car.Health.CarHealthChanged -= OnCarHealthChanged;
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
        var torque = 0f;

        if (m_CurrentAccelerationInput > 0)
        {
            torque = Car.CurrentSpeed < (Car.Stats.TopSpeed * Car.StatusManager.GetModifierAmount(Stat.Speed)) ?
            (m_CurrentAccelerationInput * Car.Stats.MotorTorque.Evaluate(Car.CurrentSpeedRatio) * Car.StatusManager.GetModifierAmount(Stat.Acceleration))
            : 0;
        }

        else if (m_CurrentBrakeInput > 0)
        {
            if (Car.HeadingDirection != CarDirection.Forward)
            {
                torque = -(Car.CurrentSpeed > -(Car.Stats.TopSpeed / 8) ?
                (m_CurrentBrakeInput * Car.Stats.MotorTorque.Evaluate(Car.CurrentSpeedRatio))
                : 0);
            }       
        }

        Car.Transmission.MotorTorque = torque;
    }

    private void OnRaceStateChanged(RaceStatus raceStatus)
    {
        m_RaceStatus = raceStatus;

        if (raceStatus == RaceStatus.Countdown 
            || raceStatus == RaceStatus.Race)
        {
            m_Flames.SetActive(false);
        }
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
