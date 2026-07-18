using UnityEngine;

public class StartBoostCheck : InputComponent
{
    private bool m_HasAccelerated;

    private void OnAccelerationPerformed(float obj)
    {
        if (m_HasAccelerated)
        {
            return;
        }

        if (RaceManager.Current.RaceStatus == RaceStatus.Race && RaceManager.Current.RaceTimer < 0.5)
        {
            Car.StatusManager.AddTimedModifier(Stat.Acceleration, ModifierType.Multiplier, 1.2f, 1f);
            Car.StatusManager.AddTimedModifier(Stat.Speed, ModifierType.Multiplier, 1.2f, 1f);
        }

        m_HasAccelerated = true;
    }

    private void OnRaceStateChanged(RaceStatus raceStatus)
    {
        if (raceStatus == RaceStatus.Countdown)
        {
            m_HasAccelerated = false;
        }
    }

    protected override void AddListeners()
    {
        Car.InputManager.Accelerate += OnAccelerationPerformed;
        RaceManager.Current.RaceStatusChanged += OnRaceStateChanged;
    }

    protected override void RemoveListeners()
    {
        Car.InputManager.Accelerate -= OnAccelerationPerformed;
        RaceManager.Current.RaceStatusChanged -= OnRaceStateChanged;
    }
}
