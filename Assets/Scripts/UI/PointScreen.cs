using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointScreen : MonoBehaviour
{
    [SerializeField]
    private PointsPanel[] m_PointsPanels;

    private void Start()
    {
        AddListeners();    
    }

    public void UpdatePoints(Car car, int newPoints)
    {
        var pointsScreen = m_PointsPanels.Where(x => x.Car == car).FirstOrDefault();

        if (pointsScreen == null)
        {
            return;
        }

        pointsScreen.SetNewPoints(newPoints);
    }

    public void SetCarPanel(Car car, bool active)
    {
        var pointsScreen = m_PointsPanels.Where(x => x.Car == car).FirstOrDefault();

        if (pointsScreen == null)
        {
            return;
        }

        pointsScreen.SetCarPanel(active);
    }

    public void SetupCars(List<PlayerCar> cars, int startPoints)
    {
        for (var i = 0; i < m_PointsPanels.Length; i++)
        {
            if (cars.Count > i)
            {
                m_PointsPanels[i].AddCar(cars[i], startPoints);
                m_PointsPanels[i].gameObject.SetActive(true);
            }
            else
            {
                m_PointsPanels[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnRaceState(RaceStatus raceState)
    {
        switch (raceState)
        {
            case RaceStatus.HeatEnd:
                break;    
            default:
                break;
        }
    }

    private void AddListeners()
    {
        RaceManager.Current.RaceStatusChanged += OnRaceState;
    }

    private void RemoveListeners()
    {
        RaceManager.Current.RaceStatusChanged -= OnRaceState;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
