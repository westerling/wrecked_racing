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
        Debug.Log("Fuck with " + amount + " amount.");

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

    private void ResetCarParts()
    {
        foreach (var carPart in Car.CarParts)
        {
            carPart.ResetTransformAndPosition();
        }
    }

    private void CheckDamage()
    {
        if (m_Health <= 0)
        {
            CarActive?.Invoke(CarStatus.Inactive);
        }

        foreach (var carPart in Car.CarParts)
        {
            if (!carPart.Detached)
            {
                if (m_Health < carPart.Threshold)
                {
                    carPart.SeperateComponent(true);
                }
            }
        }
    }

    private void OnRaceStateChanged(RaceStatus raceState)
    {
        if (raceState == RaceStatus.Countdown)
        {
            ResetHealth();
            ResetCarParts();
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
