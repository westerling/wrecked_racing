using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager Current;

    private readonly Dictionary<Car, int> m_PlayerPoints = new Dictionary<Car, int>();

    private void Awake()
    {
        Current = this;
    }

    public void SetStartPoints(List<PlayerCar> cars)
    {
        var startPoints = Globals.StartPoints(cars.Count);

        UIManager.Current.SetScreenActive(Screens.PointScreen, true);
        UIManager.Current.SetupPointScreen(cars, startPoints);

        foreach (var car in cars)
        {
            m_PlayerPoints.Add(car, startPoints);
        }
    }

    public bool CheckForWinner()
    {
        foreach (var playerPoint in m_PlayerPoints)
        {
            if (playerPoint.Value >= Globals.MaxPoints(m_PlayerPoints.Count))
            {
                return true;
            }
        }

        return false;
    }

    public void UpdatePoints(List<PlayerCar> pointCars, int carsLeft)
    {
        foreach (var car in pointCars)
        {
            var carPoints = CalculatePoints(carsLeft + 1, m_PlayerPoints[car], m_PlayerPoints.Count, pointCars.Count);
            UIManager.Current.UpdatePoints(car, carPoints);
        }
    }

    private int CalculatePoints(int position, int currentPoints, int numberOfPlayers, int numberOfCarsSharingPosition)
    {
        switch (numberOfPlayers)
        {
            case 2:
                return CalculatePointsTwoPlayers(position, currentPoints);
            case 3:
                return CalculatePointsThreePlayers(position, currentPoints);
            case 4:
                return CalculatePointsFourPlayers(position, currentPoints);
        }

        return 0;
    }

    private int CalculatePointsTwoPlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 1:
                return 1;
            case 2:
                return currentPoints == 0 ? 0 : -1;
        }

        return 0;
    }

    private int CalculatePointsThreePlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 1:
                return 1;
            case 2:
                return 0;
            case 3:
                return currentPoints == 0 ? 0 : -1;
        }

        return 0;
    }

    private int CalculatePointsFourPlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 1:
                return currentPoints == 9 ? 1 : 2;
            case 2:
                return currentPoints == 9 ? 0 : 1;
            case 3:
                return currentPoints == 0 ? 0 : -1;
            case 4:
                switch (currentPoints)
                {
                    case 0:
                        return 0;
                    case 1:
                        return -1;
                }
                return -2;
        }

        return 0;
    }
}
