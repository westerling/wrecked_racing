using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Current;

    public event Action<RaceStatus> RaceStatusChanged;
    public event Action<GameObject> LeaderChanged;

    [SerializeField]
    private Countdown m_Countdown;

    [SerializeField]
    private CinemachineTargetGroup m_TargetGroup;

    [SerializeField]
    private Transform m_CarTransform;

    [SerializeField]
    private Transform m_CameraTransform;

    [SerializeField]
    private Transform m_PoolTransform;

    private float m_RaceTimer = 0f;
    
    private RaceStatus m_RaceStatus;

    private Checkpoint m_NextCheckpoint;
    private Checkpoint m_LastCheckpoint;
    private RaceSettings m_RaceSettings;
    private List<Car> m_AllCars = new List<Car>();
    private List<Car> m_ActiveCars = new List<Car>();
    private List<Car> m_InactiveCars = new List<Car>();
    private GameObject m_Camera;
    private GameObject m_Leader;

    private readonly Dictionary<Car, int> m_PlayerPoints = new Dictionary<Car, int>();
    private readonly Dictionary<Car, float> m_HeatResults = new Dictionary<Car, float>();

    private IPointService m_PointService;

    public List<Car> Cars 
    {
        get => m_AllCars; 
        private set => m_AllCars = value; 
    }

    public GameObject Leader
    {
        get => m_Leader;
        private set
        {
            m_Leader = value;
            LeaderChanged?.Invoke(value);
        }
    }

    public RaceStatus RaceStatus
    {
        get => m_RaceStatus;
        private set
        {
            m_RaceStatus = value;
            RaceStatusChanged?.Invoke(value);
        }
    }

    public float RaceTimer
    {
        get => m_RaceTimer;
        private set => m_RaceTimer = value;
    }

    public Countdown Countdown
    {
        get => m_Countdown;
        set => m_Countdown = value;
    }

    public CinemachineTargetGroup TargetGroup
    {
        get => m_TargetGroup;
    }

    private void Awake()
    {
        Current = this;

        m_PointService = new PointService();

        SpawnPools();
        SetupCamera();
        SetupRace();
    }

    private void Start()
    {
        ResetRace();
    }

    private void Update()
    {
        if (RaceStatus == RaceStatus.Race)
        {
            SetRaceTimer();
            FindLeader();
            RemoveDistantCars();
            CheckRaceStatus();
        }
    }

    private void SetupCamera()
    {
        Instantiate(GameManager.Current.FollowCamera, m_CameraTransform);

        m_Camera = Instantiate(GameManager.Current.RaceCamera, m_CameraTransform);

        if (m_Camera.TryGetComponent(out CameraControl cameraControl))
        {
            cameraControl.SetTargetGroup(m_TargetGroup);
        }
    }

    private void SpawnPools()
    {
        foreach (var pool in GameManager.Current.Pools)
        {
            Instantiate(pool, m_PoolTransform);
        }
    }
   
    private void CheckRaceStatus()
    {
        if (GameManager.Current.DebugMode)
        {
            if (m_ActiveCars.Count < 1)
            {
                RaceFinished();
            }

            return;
        }

        if (m_ActiveCars.Count <= 1)
        {
            RaceFinished();
        }
    }

    private void RaceFinished()
    {
        RaceStatus = RaceStatus.HeatEnd;
        StopCurrentSong();
        ClearTargetGroup();
        AddGameobjectToTargetGroup(GetHeatWinner(), 1, 1);
        GivePoints();
        CheckForWinner();
        StartCoroutine(StartPauseEnumerator(5));
    }

    private GameObject GetHeatWinner()
    {
        if (m_HeatResults.Any())
        {
            return m_HeatResults.OrderBy(x => x.Value).First().Key.gameObject;
        }

        return null;
    }

    private void GivePoints()
    {
        var sortedEliminations = m_HeatResults.OrderBy(x => x.Value).ToList();
        var results = new List<(Car, int)>();
        var currentPosition = 1;
        var lastEliminationTime = sortedEliminations[0].Value;

        foreach (var (car, eliminationTime) in sortedEliminations)
        {
            if (eliminationTime - lastEliminationTime > 0.5f)
            {
                currentPosition = results.Count + 1;
            }

            results.Add((car, currentPosition));
            lastEliminationTime = eliminationTime;
        }

        foreach (var (car, position) in results)
        {
            var currentPoints = m_PlayerPoints[car];

            m_PointService.CalculatePoints(position, currentPoints, m_AllCars.Count);
        }      
    }

    private void AddObjectsToTargetGroup()
    {
        foreach (var car in m_ActiveCars)
        {
            AddGameobjectToTargetGroup(car.gameObject, 1f, 1f);
        }
    }

    private void PlayNextSong()
    {
        MusicManager.Current.AddToQueue(TrackInformationManager.Current.SongList, true);
        MusicManager.Current.Play();
    }

    private void ResumeMusic()
    {
        MusicManager.Current.ResumeMusic();
    }

    private void PauseMusic()
    {
        MusicManager.Current.PauseMusic();
    }

    private void StopCurrentSong()
    {
        MusicManager.Current.StopMusic();
    }

    private void CheckForWinner()
    {
        foreach (var playerPoint in m_PlayerPoints)
        {
            if (playerPoint.Value >= Globals.MaxPoints(m_AllCars.Count))
            {
                GameManager.Current.UnloadTrack();
            }
        }
    }

    private void SetRaceTimer()
    {
        if (RaceStatus == RaceStatus.Race)
        {
            RaceTimer += Time.deltaTime;
        }
    }

    private void FindLeader()
    {
        if (m_ActiveCars.Any())
        {
            var newLeader = m_ActiveCars.OrderBy(car => (car.transform.position - m_NextCheckpoint.transform.position).sqrMagnitude).FirstOrDefault().gameObject;

            if (newLeader == Leader)
            {
                return;
            }

            Leader = newLeader;
        }
    }

    private void RemoveDistantCars()
    {
        if (Leader == null)
        {
            return;
        }

        var inactiveCars = m_ActiveCars.Where(x => Vector3.Distance(x.transform.position, Leader.transform.position) > 40f).ToList();

        foreach (var inactiveCar in inactiveCars)
        {
            if (inactiveCar.TryGetComponent(out Health health))
            {
                health.Destroy();
            }
        }
    }

    private void SetupRace()
    {
        GetSettings();
        SpawnCars();
        SetStartPoints();
        AddListeners();
    }

    private void SetStartPoints()
    {
        var startPoints = Globals.StartPoints(m_AllCars.Count);

        UIManager.Current.SetScreenActive(Screens.PointScreen, true);
        UIManager.Current.SetutPointScreen(m_AllCars, startPoints);

        foreach (var car in m_AllCars)
        {
            m_PlayerPoints.Add(car, startPoints);
        }
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
        foreach (var car in m_AllCars)
        {
            car.CarStatusChanged += OnCarStatusChanged;
        }

        m_Countdown.CountdownEvent += OnCountdownEvent;
    }

    private void OnCountdownEvent(CountdownEvents countdownEvent)
    {
        if (countdownEvent == CountdownEvents.Start)
        {
            RaceStatus = RaceStatus.Race;
            ResumeMusic();
        }
    }

    private void OnCarStatusChanged(Car car, CarStatus carStatus)
    {
        if (car is PlayerCar playerCar)
        {
            if (carStatus == CarStatus.Inactive)
            {
                if (m_ActiveCars.Contains(car))
                {
                    m_ActiveCars.Remove(playerCar);
                }

                if (!m_InactiveCars.Contains(car))
                {
                    m_InactiveCars.Add(playerCar);
                }

                if (!m_HeatResults.ContainsKey(car))
                {
                    m_HeatResults.Add(car, m_RaceTimer);
                }

                RemoveGameobjectFromTargetGroup(car.gameObject);
            }
        }
    }

    private void ResetRace()
    {
        ResetCheckpoints();
        ResetCarLists();
        ResetCars();
        StartCountdown();
        ClearTargetGroup();
        AddObjectsToTargetGroup();
        PlayNextSong();
        PauseMusic();
    }

    private void ResetCarLists()
    {
        m_ActiveCars.AddRange(m_AllCars);
        m_InactiveCars.Clear();
        m_HeatResults.Clear();
    }

    private void StartCountdown()
    {
        RaceTimer = 0f;
        RaceStatus = RaceStatus.Countdown;
        Countdown.StartCountdown();
    }

    private void ResetCheckpoints()
    {
        RemoveCheckpointListener();
        FindPreviousStartCheckpoint();
        FindNextCheckpoint();
        AddCheckpointListener();
    }

    private void ResetCars()
    {
        if (m_LastCheckpoint == null)
        {
            return;
        }

        if (m_LastCheckpoint.TryGetComponent(out StartCheckpoint startCheckpoint))
        {
            for (var i = 0; i < Cars.Count; i++)
            {
                Cars[i].SetCarStationary(
                    startCheckpoint.StartPositions[i].transform.position,
                    startCheckpoint.StartPositions[i].transform.rotation);
            }
        }

        if (Cars.Count > 0)
        {
            Leader = Cars[0].gameObject;
        }
    }

    private void SpawnCars()
    {
        foreach (var player in GameManager.Current.Players)
        {
            var carGameObject = Instantiate(m_RaceSettings.Car, m_CarTransform);

            if (carGameObject.TryGetComponent(out PlayerCar car))
            {
                Cars.Add(car);
                car.Player = player;

                if (player.TryGetComponent(out InputManager inputManager))
                {
                    car.InputManager = inputManager;
                }
            }
        }
    }

    private void AddGameobjectToTargetGroup(GameObject newTargetObject, float weight, float radius)
    {
        if (m_Camera.TryGetComponent(out CameraControl cameraControl))
        {
            cameraControl.AddToTargetGroup(newTargetObject, weight, radius);
        }
    }

    private void RemoveGameobjectFromTargetGroup(GameObject objectToBeRemoved)
    {
        if (m_Camera.TryGetComponent(out CameraControl cameraControl))
        {
            cameraControl.RemoveFromTargetGroup(objectToBeRemoved);
        }
    }

    private void ClearTargetGroup()
    {
        if (m_Camera.TryGetComponent(out CameraControl cameraControl))
        {
            cameraControl.ClearTargetGroup();
        }
    }

    private IEnumerator StartPauseEnumerator(int delay)
    {
        yield return new WaitForSeconds(delay);

        ResetRace();
    }

    private void RemoveListeners()
    {
        foreach (var car in m_AllCars)
        {
            if (car != null)
            {
                car.CarStatusChanged -= OnCarStatusChanged;
            }
        }

        m_Countdown.CountdownEvent -= OnCountdownEvent;
    }

    private void OnCheckpointPassed(Checkpoint checkpointPassed)
    {
        RemoveCheckpointListener();
        m_LastCheckpoint = m_NextCheckpoint;
        FindNextCheckpoint();
        AddCheckpointListener();
    }

    private void FindNextCheckpoint()
    {
        var checkpoints = TrackInformationManager.Current.Checkpoints;

        if (m_LastCheckpoint == null)
        {
            m_LastCheckpoint = checkpoints[0];
        }

        var index = Array.IndexOf(checkpoints, m_LastCheckpoint);

        if (index < checkpoints.Length - 1)
        {
            index++;
            m_NextCheckpoint = checkpoints[index];
        }
        else
        {
            m_NextCheckpoint = checkpoints[0];
        }

        m_NextCheckpoint.gameObject.SetActive(true);
    }

    private void FindPreviousStartCheckpoint()
    {
        var checkpoints = TrackInformationManager.Current.Checkpoints;

        for (var i = checkpoints.Length - 1; i >= 0; i--)
        {
            if (checkpoints[i].TryGetComponent(out StartCheckpoint _))
            {
                m_LastCheckpoint = checkpoints[i];
                return;
            }
        }

        Application.Quit();
    }

    //private void FindPreviousStartCheckpointOld()
    //{
    //    if (m_LastCheckpoint == null)
    //    {
    //        m_LastCheckpoint = m_Checkpoints[0];
    //    }

    //    var startingCheckpoints = m_Checkpoints.Where(x => x.TryGetComponent(out StartCheckpoint startCheckpoint)).ToList();

    //    if (startingCheckpoints.Count == 0)
    //    {
    //        Application.Quit();
    //    }

    //    var index = Array.IndexOf(m_Checkpoints, m_LastCheckpoint);

    //    while (m_LastCheckpoint == null)
    //    {
    //        if (startingCheckpoints.Contains(m_Checkpoints[index]))
    //        {
    //            m_LastCheckpoint = m_Checkpoints[index];
    //        }
    //        else
    //        {
    //            index--;

    //            if (index < 0)
    //            {
    //                index = m_Checkpoints.Length - 1;
    //            }
    //        }
    //    }
    //}

    private void AddCheckpointListener()
    {
        if (m_NextCheckpoint == null)
        {
            return;
        }

        m_NextCheckpoint.CheckpointPassed += OnCheckpointPassed;
    }

    private void RemoveCheckpointListener()
    {
        if (m_NextCheckpoint == null)
        {
            return;
        }

        m_NextCheckpoint.CheckpointPassed -= OnCheckpointPassed;
    }

    private void Quit()
    {
        UIManager.Current.SetScreenActive(Screens.PointScreen, false);
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}