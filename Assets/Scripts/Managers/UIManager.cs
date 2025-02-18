using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Current;

    [Header("Screens")]
    [SerializeField]
    private GameObject m_SplashScreen;

    [SerializeField]
    private GameObject m_LoadingScreen;

    [SerializeField]
    private GameObject m_CountDownScreen;

    [SerializeField]
    private GameObject m_PointScreen;

    public void SetScreenActive(Screens screen, bool active)
    {
        switch (screen)
        {
            case Screens.SplashScreen:
                m_SplashScreen.SetActive(active);
                break;
            case Screens.LoadingScreen:
                m_LoadingScreen.SetActive(active);
                break;
            case Screens.PointScreen:
                m_PointScreen.SetActive(active);
                break;
        }
    }

    public void SetutPointScreen(List<Car> cars, int startPoints)
    {
        if (m_PointScreen.TryGetComponent(out PointScreen pointScreen))
        {
            pointScreen.SetupCars(cars, startPoints);
        }
    }

    public void UpdatePoints(Car car, int points)
    {
        if (m_PointScreen.TryGetComponent(out PointScreen pointScreen))
        {
            pointScreen.UpdatePoints(car, points);
        }
    }

    private void Awake()
    {
        Current = this;
    }
}
