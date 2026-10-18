using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject m_PointScreen;

    [SerializeField]
    private GameObject m_AirstrikeScreen;

    [SerializeField]
    private GameObject m_BlackScreen;

    private void Awake()
    {
        Current = this;
    }

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
            case Screens.BlackScreen:
                m_BlackScreen.SetActive(active);
                break;
        }
    }

    public void AddRaceListeners()
    {
        AddListeners();
    }

    public void RemoveAllScreens()
    {
        m_SplashScreen.SetActive(false);
        m_LoadingScreen.SetActive(false);
        m_PointScreen.SetActive(false);
        m_AirstrikeScreen.SetActive(false);
        m_BlackScreen.SetActive(false);
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

    private IEnumerator FadeCamera(Image image, bool fade)
    {
        yield return Fade(image, 0f, 1f, 0.2f);
        yield return new WaitForSeconds(0.5f);
        yield return Fade(image, 1f, 0f, 0.2f);
    }

    private IEnumerator Fade(Image image, float from, float to, float duration)
    {
        var color = image.color;
        color.a = from;
        image.color = color;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(from, to, elapsed / duration);
            image.color = color;

            yield return null;
        }

        color.a = to;
        image.color = color;
    }

    private void OnCameraStatusChanged(CameraStatus status)
    {
        var image = m_BlackScreen.GetComponentInChildren<Image>();

        if (image == null)
        {
            Debug.LogError("No image on image");
        }

        switch (status)
        {
            case CameraStatus.CameraCutIn:
                StartCoroutine(Fade(image, 1f, 0f, 0.2f));
                break;
            case CameraStatus.CameraCutOut:
                StartCoroutine(Fade(image, 0f, 1f, 0.2f));
                break;
        }
    }

    private void OnRaceStatusChanged(RaceStatus raceStatus)
    {
        if (raceStatus == RaceStatus.Finished)
        {
            var image = m_BlackScreen.GetComponentInChildren<Image>();

            if (image == null)
            {
                Debug.LogError("No image on image");
            }

            StartCoroutine(Fade(image, 1f, 0f, 0.2f));
            RemoveListeners();
        }
    }

    private void AddListeners()
    {
        if (RaceManager.Current != null)
        {
            RaceManager.Current.RaceStatusChanged += OnRaceStatusChanged;
            RaceManager.Current.CameraStatusChanged += OnCameraStatusChanged;
        }
    }

    private void RemoveListeners()
    {
        if (RaceManager.Current != null)
        {
            RaceManager.Current.RaceStatusChanged -= OnRaceStatusChanged;
            RaceManager.Current.CameraStatusChanged -= OnCameraStatusChanged;
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
