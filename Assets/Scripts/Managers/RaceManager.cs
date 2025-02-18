using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Current;

    public event Action<RaceStatus> OnRaceStatus;
    public event Action<GameObject> LeaderChange;

    [SerializeField]
    private Countdown m_Countdown;

    [SerializeField]
    private Checkpoint[] m_Checkpoints;

    [SerializeField]
    private GameObject m_ScoreBoard;

    private float m_RaceTimer = 0f;
    
    private RaceStatus m_RaceStatus;

    private Checkpoint m_NextCheckpoint;
    private Checkpoint m_LastCheckpoint;
    private RaceSettings m_RaceSettings;
    private List<Car> m_Cars = new List<Car>();
    private List<Car> m_ActiveCars = new List<Car>();
    private List<Car> m_InactiveCars = new List<Car>();
    private Dictionary<Car, int> m_PlayerPoints = new Dictionary<Car, int>();
    private Dictionary<Car, float> m_HeatResults = new Dictionary<Car, float>();
    private GameObject m_Camera;
    private GameObject m_Leader;

    private IPointService m_PointService;

    public List<Car> Cars 
    {
        get => m_Cars; 
        private set => m_Cars = value; 
    }

    public GameObject Leader
    {
        get => m_Leader;
        set => m_Leader = value;
    }

    public RaceStatus RaceStatus
    {
        get => m_RaceStatus;
        private set
        {
            OnRaceStatus?.Invoke(value);
            m_RaceStatus = value;
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

    private void Awake()
    {
        Current = this;

        m_PointService = new PointService();

        SetupCamera();
    }

    private void SetupCamera()
    {
        Instantiate(GameManager.Current.FollowCamera);

        m_Camera = Instantiate(GameManager.Current.RaceCamera);

        if (m_Camera.TryGetComponent(out FollowCamera followCamera))
        {
            followCamera.SetTargetGroup();
        }
    }

    private void Start()
    {
        SetupRace();
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

    private void CheckRaceStatus()
    {
        if (m_ActiveCars.Count < 1)
        {
            RaceFinished();
        }
    }

    private void RaceFinished()
    {
        RaceStatus = RaceStatus.HeatEnd;
        AddObjectsToTargetGroup();
        CheckForWinner();
        StartCoroutine(StartPauseEnumerator(5));
    }

    private void AddObjectsToTargetGroup()
    {
        ClearTargetGroup();

        switch (RaceStatus)
        {
            case RaceStatus.HeatEnd:
            case RaceStatus.Finished:
                
                break;
            case RaceStatus.Countdown:
            case RaceStatus.Race:
                foreach (var car in m_ActiveCars)
                {
                    AddGameobjectToTargetGroup(car.gameObject, 1f, 1f);
                }
                break;
        }
    }

    private void CheckForWinner()
    {
        foreach (var playerPoint in m_PlayerPoints)
        {
            if (playerPoint.Value >= Globals.MaxPoints(m_Cars.Count))
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
            LeaderChange?.Invoke(newLeader);
        }
    }

    private void RemoveDistantCars()
    {
        var inactiveCars = m_ActiveCars.Where(x => Vector3.Distance(x.transform.position, Leader.transform.position) > 25f).ToList();

        foreach (var car in inactiveCars)
        {
            if (car.TryGetComponent(out Health health))
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
        AddPodiumToCamera();
    }

    private void SetStartPoints()
    {
        var startPoints = Globals.StartPoints(m_Cars.Count);

        UIManager.Current.SetScreenActive(Screens.PointScreen, true);
        UIManager.Current.SetutPointScreen(m_Cars, startPoints);

        foreach (var car in m_Cars)
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
        foreach (var car in m_Cars)
        {
            car.CarActive += OnCarActive;
        }

        m_Countdown.CountdownEvent += OnCountdownEvent;
    }

    private void OnCountdownEvent(CountdownEvents countdownEvent)
    {
        if (countdownEvent == CountdownEvents.Start)
        {
            RaceStatus = RaceStatus.Race;
        }
    }

    private void OnCarActive(Car car, bool active)
    {
        if (!active)
        {
            if (m_InactiveCars.Contains(car) || !m_ActiveCars.Contains(car))
            {
                return;
            }

            m_InactiveCars.Add(car);
            m_ActiveCars.Remove(car);

            m_HeatResults.Add(car, m_RaceTimer);

            RemoveGameobjectFromTargetGroup(car.gameObject);
        }
    }

    private void ResetRace()
    {
        ResetCheckpoints();
        ResetCarLists();
        ResetCars();
        StartCountdown();
        AddObjectsToTargetGroup();
    }

    private void ResetCarLists()
    {
        m_ActiveCars.AddRange(m_Cars);
        m_InactiveCars.Clear();
        m_HeatResults.Clear();
    }

    private void StartCountdown()
    {
        Debug.Log("start countdown");

        if (m_Camera.TryGetComponent(out CountdownCamera countdownCamera))
        {
            countdownCamera.SetTarget(m_LastCheckpoint.gameObject);
        }

        RaceTimer = 0f;
        RaceStatus = RaceStatus.Countdown;
        Countdown.StartCountdown();
    }

    private void ResetCheckpoints()
    {
        FindPreviousStartCheckpoint();
        FindNextCheckpoint();
    }

    private void ResetCars()
    {
        if (m_LastCheckpoint == null)
        {
            Debug.Log("No Last Checkpoint");
            return;
        }

        

        if (m_LastCheckpoint.TryGetComponent(out StartCheckpoint startCheckpoint))
        {
            for (var i = 0; i < Cars.Count; i++)
            {
                Cars[i].transform.position = startCheckpoint.StartPositions[i].transform.position;
                Cars[i].transform.rotation = startCheckpoint.StartPositions[i].transform.rotation;
                Cars[i].ResetCar();
            }
        }
    }

    private void SpawnCars()
    {
        foreach (var player in GameManager.Current.Players)
        {
            var carGameObject = Instantiate(m_RaceSettings.Car);

            if (carGameObject.TryGetComponent(out Car car))
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

    private void AddPodiumToCamera()
    {
        if (m_Camera.TryGetComponent(out PodiumCamera podiumCamera))
        {
            podiumCamera.SetTarget(m_ScoreBoard);
        }
    }

    private void AddGameobjectToTargetGroup(GameObject newTargetObject, float weight, float radius)
    {
        if (m_Camera.TryGetComponent(out FollowCamera followCamera))
        {
            followCamera.AddToTargetGroup(newTargetObject, weight, radius);
        }
    }

    private void RemoveGameobjectFromTargetGroup(GameObject objectToBeRemoved)
    {
        if (m_Camera.TryGetComponent(out FollowCamera followCamera))
        {
            followCamera.RemoveFromTargetGroup(objectToBeRemoved);
        }
    }

    private void ClearTargetGroup()
    {
        if (m_Camera.TryGetComponent(out FollowCamera followCamera))
        {
            followCamera.ClearTargetGroup();
        }
    }

    private IEnumerator StartPauseEnumerator(int delay)
    {
        yield return new WaitForSeconds(delay);

        ResetRace();
    }

    private void RemoveListeners()
    {
        foreach (var car in m_Cars)
        {
            if (car != null)
            {
                car.CarActive -= OnCarActive;
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
        if (m_LastCheckpoint == null)
        {
            m_LastCheckpoint = m_Checkpoints[0];
        }

        var index = Array.IndexOf(m_Checkpoints, m_LastCheckpoint);

        if (index < m_Checkpoints.Length - 1)
        {
            index++;
            m_NextCheckpoint = m_Checkpoints[index];
        }
        else
        {
            m_NextCheckpoint = m_Checkpoints[0];
        }

        m_NextCheckpoint.gameObject.SetActive(true);
    }

    private void FindPreviousStartCheckpoint()
    {
        if (m_LastCheckpoint == null)
        {
            m_LastCheckpoint = m_Checkpoints[0];
        }

        var startingCheckpoints = m_Checkpoints.Where(x => x.TryGetComponent(out StartCheckpoint startCheckpoint)).ToList();

        if (startingCheckpoints.Count == 0)
        {
            Application.Quit();
        }

        var index = Array.IndexOf(m_Checkpoints, m_LastCheckpoint);

        while (m_LastCheckpoint == null)
        {
            if (startingCheckpoints.Contains(m_Checkpoints[index]))
            {
                m_LastCheckpoint = m_Checkpoints[index];
            }
            else
            {
                index--;

                if (index < 0)
                {
                    index = m_Checkpoints.Length - 1;
                }
            }
        }
    }

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
