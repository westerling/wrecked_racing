using System;
using UnityEngine;

public class Health : CarComponent
{
    public event Action<CarStatus> CarActive;

    private float m_Health;

    protected override void Awake()
    {
        base.Awake();

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
        CarActive?.Invoke(CarStatus.Active);
    }

    private void CheckDamage()
    {
        if (m_Health <= 0)
        {
            CarActive?.Invoke(CarStatus.Inactive);
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
