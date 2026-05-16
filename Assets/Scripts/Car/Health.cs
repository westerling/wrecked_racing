using System;

public class Health : CarComponent
{
    public event Action<CarStatus> CarHealthStatus;
    public event Action<float, float> CarHealthChanged;

    private float m_MaxHealthPoints;
    private float m_HealthPoints;

    public float HealthPoints
    {
        get => m_HealthPoints;
        set => m_HealthPoints = value;
    }

    protected override void Awake()
    {
        base.Awake();

        m_MaxHealthPoints = Car.Stats.HealthPoints;

        AddListeners();
    }

    public void Damage(float amount)
    {
        HealthPoints -= amount;
        var healthRatio = HealthPoints / m_MaxHealthPoints;
        CarHealthChanged?.Invoke(HealthPoints, healthRatio);
        CheckDamage();
    }

    private void ResetHealth()
    {
        HealthPoints = m_MaxHealthPoints;
        var healthRatio = HealthPoints / m_MaxHealthPoints;
        CarHealthChanged?.Invoke(HealthPoints, healthRatio);
        CarHealthStatus?.Invoke(CarStatus.Active);
    }

    private void CheckDamage()
    {
        if (HealthPoints <= 0)
        {
            CarHealthStatus?.Invoke(CarStatus.Inactive);
        }
    }

    private void OnRaceStateChanged(RaceStatus raceState)
    {
        if (raceState == RaceStatus.Countdown)
        {
            ResetHealth();
        }
    }


    private void AddListeners()
    {
        RaceManager.Current.RaceStatusChanged += OnRaceStateChanged;
    }

    private void RemoveListeners()
    {
        RaceManager.Current.RaceStatusChanged -= OnRaceStateChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
