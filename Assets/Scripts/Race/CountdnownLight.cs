using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CountdnownLight : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> m_RedLights = new List<GameObject>();

    [SerializeField]
    private List<GameObject> m_OrangeLights = new List<GameObject>();

    [SerializeField]
    private List<GameObject> m_GreenLights = new List<GameObject>();

    private void Start()
    {
        AddListeners();
        TurnOffAllLights();
    }

    private void OnCountdownTimer(CountdownEvents countdownEvent)
    {
        switch (countdownEvent)
        {
            case CountdownEvents.RedLights:
                SwitchLights(m_RedLights, true);
                break;
            case CountdownEvents.YellowLights:
                SwitchLights(m_OrangeLights, true);
                break;
            case CountdownEvents.Start:
                SwitchLights(m_RedLights, false);
                SwitchLights(m_OrangeLights, false);
                SwitchLights(m_GreenLights, true);
                break;
            case CountdownEvents.LightsOut:
                TurnOffAllLights();
                break;
        }
    }

    private void TurnOffAllLights()
    {
        SwitchLights(m_RedLights, false);
        SwitchLights(m_OrangeLights, false);
        SwitchLights(m_GreenLights, false);
    }

    private void SwitchLights(List<GameObject> lights, bool switchOn)
    {
        foreach (var light in lights)
        {
            light.SetActive(switchOn);
        }
    }

    private void AddListeners()
    {
        RaceManager.Current.Countdown.CountdownEvent += OnCountdownTimer;
    }

    private void RemoveListeners()
    {
        RaceManager.Current.Countdown.CountdownEvent -= OnCountdownTimer;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
