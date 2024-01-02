using System;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField]
    private Checkpoint[] m_Checkpoints;

    private Checkpoint m_NextCheckpoint;
    private Checkpoint m_LastCheckpoint;
    private RaceSettings m_RaceSettings;
    private List<CarRaceStats> m_RaceStats = new List<CarRaceStats>();

    //private List<Car> m_Cars = new List<Car>();

    public static RaceManager Current;

    public List<CarRaceStats> RaceStats 
    {
        get => m_RaceStats; 
        private set => m_RaceStats = value; 
    }

    private void Awake()
    {
        Current = this;

        var camera = Instantiate(GameManager.Current.Camera);
    }

    private void Start()
    {
        ResetRace();
    }

    private void SetupRace()
    {
        GetSettings();
        SpawnCars();
        AddListeners();
    }

    private void GetSettings()
    {
        if (GameManager.Current.RaceSettings != null)
        {
            m_RaceSettings = GameManager.Current.RaceSettings;
            return;
        }

        Application.Quit();
    }

    private void AddListeners()
    {
    }

    private void ResetRace()
    {

    }

    private void SpawnCars()
    {
        foreach (var player in GameManager.Current.Players)
        {
            var carGameObject = Instantiate(m_RaceSettings.Car);

            m_RaceStats.Add(new CarRaceStats
            {
                Car = carGameObject,
                IsActive = false,
                Points = 0
            });
        }
    }
}
