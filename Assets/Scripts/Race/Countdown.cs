using System;
using System.Collections;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    public event Action<int> CountdownTimer;
    public event Action<CountdownEvents> CountdownEvent;

    public void StartCountdown()
    {
        StartCoroutine(StartCountdownEnumerator(8));
    }

    private IEnumerator StartCountdownEnumerator(int seconds)
    {
        CountdownEvent?.Invoke(CountdownEvents.Preparation);
        var count = seconds;

        while (count >= 0)
        {
            if (count == 5)
            {
                CountdownEvent?.Invoke(CountdownEvents.RedLights);
            }

            if (count == 4)
            {
                CountdownEvent?.Invoke(CountdownEvents.YellowLights);
            }

            if (count == 3)
            {
                CountdownEvent?.Invoke(CountdownEvents.Start);
            }

            if (count == 0)
            {
                CountdownEvent?.Invoke(CountdownEvents.LightsOut);
            }

            CountdownTimer?.Invoke(count);

            yield return new WaitForSeconds(1);
            count--;
        }
    }
}
