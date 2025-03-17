using System;

public class Health : CarComponent
{
    public event Action<bool> CarActive;

    private float m_Health;
    
    void Start()
    {
        AddListeners();
    }

    public void Damage(float amount)
    {
        m_Health -= amount;
        CheckDamage();
    }

    public void Destroy()
    {
        m_Health = 0;
        CheckDamage();
    }

    private void ResetHealth()
    {
        m_Health = Car.Stats.Health;
        CarActive?.Invoke(true);
    }

    private void CheckDamage()
    {
        if (m_Health <= 0)
        {
            CarActive?.Invoke(false);
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
        RaceManager.Current.OnRaceStatus += OnRaceStateChanged;
    }

    private void RemoveListeners()
    {
        RaceManager.Current.OnRaceStatus -= OnRaceStateChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
