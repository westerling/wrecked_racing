using System;

public class Health : CarComponent
{
    public event Action<CarStatus, PlayerCar> CarHealthStatus;
    public event Action<float, float> CarHealthChanged;

    private CarStatus m_CarStatus;

    private float m_HealthPoints;

    public float HealthPoints
    {
        get => m_HealthPoints;
        set => m_HealthPoints = value;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public void Damage(float amount)
    {
        HealthPoints -= amount;
        var healthRatio = HealthPoints / Car.Stats.HealthPoints;
        CarHealthChanged?.Invoke(HealthPoints, healthRatio);
        CheckDamage();
    }

    public void ResetHealth()
    {
        HealthPoints = Car.Stats.HealthPoints;
        m_CarStatus = CarStatus.Active;
        CarHealthChanged?.Invoke(HealthPoints, 1);

        if (Car is PlayerCar playerCar)
        {
            CarHealthStatus?.Invoke(m_CarStatus, playerCar);
        }
    }

    private void CheckDamage()
    {
        if (m_CarStatus == CarStatus.Active)
        {
            if (HealthPoints <= 0)
            {
                m_CarStatus = CarStatus.Inactive;
                
                if (Car is PlayerCar playerCar)
                {
                    CarHealthStatus?.Invoke(m_CarStatus, playerCar);
                }
            }
        }
    }
}
