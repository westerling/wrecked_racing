using System;
using UnityEngine;

public class CarVisualsBase : CarComponent
{
    [SerializeField]
    private PlayerColor m_Color;

    [SerializeField]
    private CarPart[] m_CarParts;

    public PlayerColor Color
    {
        get => m_Color;
    }

    public CarPart[] CarParts
    {
        get => m_CarParts;
    }

    protected override void Awake()
    {
        base.Awake();
        AddListeners();

        foreach (var carPart in m_CarParts)
        {
            carPart.SetupCarPart(transform, Car);
        }
    }

    private void ResetCarParts()
    {
        foreach (var carPart in CarParts)
        {
            carPart.ResetTransformAndPosition();
        }
    }

    private void OnCarlHealthChanged(float currentHealth, float currentHealthRatio)
    {
        foreach (var carPart in m_CarParts)
        {
            if (!carPart.Detached)
            {
                if (currentHealthRatio < carPart.Threshold)
                {
                    carPart.DetachComponent(true);
                }
            }
        }
    }

    private void AddListeners()
    {
        if (Car is PlayerCar playerCar)
        {
            playerCar.Health.CarHealthChanged += OnCarlHealthChanged;
            playerCar.Health.CarHealthStatus += OnCarlHealthStatusChanged;
        }
    }

    private void OnCarlHealthStatusChanged(CarStatus status)
    {
        if (status == CarStatus.Active)
        {
            ResetCarParts();
        }
    }

    private void RemoveListeners()
    {
        if (Car is PlayerCar playerCar)
        {
            playerCar.Health.CarHealthChanged -= OnCarlHealthChanged;
            playerCar.Health.CarHealthStatus -= OnCarlHealthStatusChanged;
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
