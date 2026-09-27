using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Current;

    [SerializeField]
    private Sprite[] m_Sprites;

    [Header("Screens")]
    [SerializeField]
    private GameObject m_SplashScreen;

    [SerializeField]
    private GameObject m_LoadingScreen;

    [SerializeField]
    private GameObject m_CountDownScreen;

    [SerializeField]
    private GameObject m_PointScreen;

    [SerializeField]
    private GameObject m_PauseScreen;

    [Header("Airstrike")]
    [SerializeField]
    private GameObject m_AirstrikeScreen;

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
            case Screens.AirstrikeScreen:
                m_AirstrikeScreen.SetActive(active);
                break;
        }
    }

    public void AddIcon(IconType iconType, Transform carTransform)
    {

    }

    public void SetupPointScreen(List<PlayerCar> cars, int startPoints)
    {
        if (m_PointScreen.TryGetComponent(out PointScreen pointScreen))
        {
            pointScreen.SetupCars(cars, startPoints);
        }
    }

    public void SetupAirstrikeScreen(List<PlayerCar> cars)
    {
        if (m_AirstrikeScreen.TryGetComponent(out AirstrikeScreen airstrikeScreen))
        {
            airstrikeScreen.SetupAirstrike(cars);
        }
    }

    public void UpdatePoints(PlayerCar car, int newPoints)
    {
        if (m_PointScreen.TryGetComponent(out PointScreen pointScreen))
        {
            pointScreen.UpdatePoints(car, newPoints);
        }
    }

    public void SetCarPanel(PlayerCar car, bool active)
    {
        if (m_PointScreen.TryGetComponent(out PointScreen pointScreen))
        {
            pointScreen.SetCarPanel(car, active);
        }
    }

    private void Awake()
    {
        Current = this;
    }
}
