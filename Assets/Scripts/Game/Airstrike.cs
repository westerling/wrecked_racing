using System;
using UnityEngine;

public class Airstrike : MonoBehaviour
{
    [SerializeField]
    private AimSight m_AimSight;

    private PlayerCar m_Car;

    private bool activeMissile = false;
    private RaceStatus m_RaceStatus;

    private void Awake()
    {
        var aimSight = GetComponentInChildren<AimSight>();

        if (aimSight == null)
        {
            Debug.LogError("No AimSight attached to children.");
        }
        else
        {
            m_AimSight = aimSight;
        }
    }

    public void AddCar(PlayerCar playerCar)
    {
        m_Car = playerCar;
        
        var color = Globals.GetPlayerColor(m_Car.Player.Color, 255);
        m_AimSight.SetColor(color);

        AddListeners();
    }

    private void ToggleAirstrikeUI(bool enabled)
    {
        m_AimSight.gameObject.SetActive(enabled);
    }


    private void ActivateHomingMissile()
    {
        var target = m_AimSight.Target;

        if (target == null)
        {
            return;
        }

        var cameraTransform = Camera.main.transform;
        var rocket = GetGameObjectFromPool(AmmunitionType.HomingMissile);
        
        activeMissile = true;

        if (rocket.TryGetComponent(out HomingMissile homingMissile))
        {
            homingMissile.ActivateMissile(cameraTransform, target.transform, target.CurrentSpeed, target.Stats.TopSpeed);
            homingMissile.MissileExploded += OnMissileExploded;
        }
    }

    private void OnMissileExploded(Missile missile)
    {
        activeMissile = false;
        missile.MissileExploded -= OnMissileExploded;

        if (m_RaceStatus == RaceStatus.Race)
        {
            ToggleAirstrikeUI(true);
        }
    }

    private GameObject GetGameObjectFromPool(AmmunitionType ammunitionType)
    {
        var rocket = AmmunitionPool.Current.GetPooledObjectOfType(ammunitionType);

        if (rocket != null)
        {
            rocket.transform.SetPositionAndRotation(Camera.main.transform.position, Camera.main.transform.rotation);
            rocket.SetActive(true);

            return rocket;
        }

        return null;
    }

    private void OnFireStarted()
   {
        if (m_AimSight.LockedIn && !activeMissile)
        {
            ActivateHomingMissile();
            ToggleAirstrikeUI(false);
        }
   }

    private void OnCarHealthStatusChanged(CarStatus carStatus, PlayerCar car)
    {
        switch (carStatus)
        {
            case CarStatus.Active:
                ToggleAirstrikeUI(false);
                break;
            case CarStatus.Inactive:
                ToggleAirstrikeUI(true);
                break;
        }
    }

    private void OnRaceStatusChanged(RaceStatus raceStatus)
    {
        m_RaceStatus = raceStatus;

        switch (raceStatus)
        {
            case RaceStatus.HeatEnd:
            case RaceStatus.Finished:
            case RaceStatus.Countdown:
                ToggleAirstrikeUI(false);
                break;
            default:
                return;
        }
    }

    private void AddListeners()
    {
        m_Car.Health.CarHealthStatus += OnCarHealthStatusChanged;
        m_Car.InputManager.FireStarted += OnFireStarted;
        m_Car.InputManager.Aim += OnAim;
        RaceManager.Current.RaceStatusChanged += OnRaceStatusChanged;
    }

    private void OnAim(Vector2 obj)
    {
        m_AimSight.AimInput = obj;
    }

    private void RemoveListeners()
    {
        if (m_Car != null)
        {
            m_Car.Health.CarHealthStatus -= OnCarHealthStatusChanged;
            m_Car.InputManager.FireStarted -= OnFireStarted;
            m_Car.InputManager.Aim -= OnAim;
        }
        
        RaceManager.Current.RaceStatusChanged -= OnRaceStatusChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
