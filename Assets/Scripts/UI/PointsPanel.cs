using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_PointsText;

    [SerializeField]
    private GameObject m_CarPanel;

    [SerializeField]
    private GameObject m_PointsPanel;

    [SerializeField]
    private GameObject m_PointsMarkersPanel;

    [SerializeField]
    private GameObject[] m_PointMarkers;

    private int m_Points;

    private const int pointCell = 46;

    private Car m_Car;

    public Car Car
    {
        get => m_Car;
        private set => m_Car = value;
    }

    public void AddCar(Car car, int startPoints)
    {
        Car = car;
        m_Points = startPoints;
    }

    public void SetNewPoints(int newPoints)
    {
        ShowNewPoints(true);

        m_PointsText.text = FormatText(newPoints);
        m_Points += newPoints;
        
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        for (var i = 0; i < m_PointMarkers.Length; i++)
        {
            m_PointMarkers[i].SetActive(m_Points > i);
        }
    }

    private void Start()
    {
        AddListeners();
        SetWidth();
    }

    private void SetWidth()
    {
        var players = RaceManager.Current.Cars.Count;

        var pointWidth = Globals.PointWidth(players);

        if (m_PointsMarkersPanel.TryGetComponent(out GridLayoutGroup gridLayoutGroup))
        {
            gridLayoutGroup.cellSize.Set(pointWidth, pointCell);
        }
    }

    private void AddListeners()
    {
        RaceManager.Current.OnRaceStatus += OnRaceState;
    }

    private void OnRaceState(RaceStatus raceState)
    {
        switch (raceState)
        {
            case RaceStatus.Countdown:
                ShowNewPoints(false);
                break;
            case RaceStatus.Race:
                ShowNewPoints(false);
                break;
            case RaceStatus.HeatEnd:
                ShowNewPoints(true);
                break;
            case RaceStatus.Finished:
                ShowNewPoints(true);
                break;
            default:
                break;
        }
    }

    private string FormatText(int newPoints)
    {
        if (newPoints >= 0)
        {
            return "+" + newPoints;
        }
        
        return newPoints.ToString();
    }

    private void ShowNewPoints(bool showPoints)
    {
        m_CarPanel.SetActive(!showPoints);
        m_PointsPanel.SetActive(showPoints);
    }

    private void RemoveListeners()
    {
        RaceManager.Current.OnRaceStatus -= OnRaceState;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
