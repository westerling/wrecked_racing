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
    private float m_DeathPoolTimer = 0f;
    private bool m_DeathPoolOpen = false;

    private RaceStatus m_RaceStatus;

    private Checkpoint m_NextCheckpoint;
    private Checkpoint m_LastCheckpoint;
    private Checkpoint m_StartCheckpoint;
    private RaceSettings m_RaceSettings;
    private List<PlayerCar> m_AllCars = new List<PlayerCar>();
    private List<PlayerCar> m_ActiveCars = new List<PlayerCar>();
    private List<PlayerCar> m_DeathPool = new List<PlayerCar>();
    private GameObject m_Camera;
    private GameObject m_Leader;

    public List<PlayerCar> Cars 
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

        SpawnPools();
        SetupCamera();
        SetupRace();
    }

    private void Start()
    {
        SetStartPoints();
        ResetRace();
    }

    private void SetStartPoints()
    {
        PointsManager.Current.SetStartPoints(m_AllCars);
    }

    private void Update()
    {
        if (RaceStatus == RaceStatus.Race)
        {
            SetDeathPoolTimer();
            CheckDeathPool();
            SetRaceTimer();
            FindLeader();
            RemoveDistantCars();
            CheckRaceStatus();
        }
    }

    private void CheckDeathPool()
    {
        if (m_DeathPoolOpen)
        {
            if (m_DeathPoolTimer <= 0)
            {
                CleanDeathPool();
            }
        }
    }

    private void CleanDeathPool()
    {
        PointsManager.Current.UpdatePoints(m_DeathPool, m_ActiveCars.Count());

        foreach (var playerCar in m_DeathPool)
        {
            if (m_ActiveCars.Contains(playerCar))
            {
                m_ActiveCars.Remove(playerCar);
            }
        }

        m_DeathPool.Clear();
        m_DeathPoolOpen = false;
    }

    public void Quit()
    {
        UIManager.Current.SetScreenActive(Screens.PointScreen, false);
        GameManager.Current.UnloadTrack();
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
        if (m_DeathPoolOpen)
        {
            return;
        }

        if (m_AllCars.Count() == 1)
        {
            if (m_ActiveCars.Count < 1)
            {
                RaceFinished();
            }

            return;
        }

        if (m_ActiveCars.Count <= 1)
        {
            PointsManager.Current.UpdatePoints(m_ActiveCars, m_ActiveCars.Count);
            RaceFinished();
        }
    }

    private void RaceFinished()
    {
        RaceStatus = RaceStatus.HeatEnd;
        StopCurrentSong();
        ClearTargetGroup();
        AddGameobjectToTargetGroup(GetHeatWinner(), 1, 1);
        CheckForWinner();
        StartCoroutine(StartPauseEnumerator(5));
    }

    private GameObject GetHeatWinner()
    {
        if (m_ActiveCars.Count() == 1)
        {
            return m_ActiveCars.FirstOrDefault().gameObject;
        }

        if (m_AllCars.Any())
        {
            return m_AllCars.FirstOrDefault().gameObject;
        }

        return null;
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
        if (PointsManager.Current.CheckForWinner())
        {
            Quit();
        }
    }

    private void SetRaceTimer()
    {
        if (RaceStatus == RaceStatus.Race)
        {
            RaceTimer += Time.deltaTime;
        }
    }

    private void SetDeathPoolTimer()
    {
        if (RaceStatus == RaceStatus.Race)
        {
            if (m_DeathPoolOpen)
            {
                m_DeathPoolTimer -= Time.deltaTime;
            }
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

        var inactiveCars = m_ActiveCars.Where(x => Vector3.Distance(x.transform.position, Leader.transform.position) > 60f).ToList();

        foreach (var inactiveCar in inactiveCars)
        {
            inactiveCar.Health.Damage(float.MaxValue);
        }
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

        Debug.LogError("No Settings Found");
        Application.Quit();
    }

    private void AddListeners()
    {
        foreach (var car in m_AllCars)
        {
            car.Health.CarHealthStatus += OnCarStatusChanged;
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

    private void OnCarStatusChanged(CarStatus carStatus, Car car)
    {
        if (car is PlayerCar playerCar)
        {
            if (carStatus == CarStatus.Inactive)
            {
                m_DeathPoolOpen = true;
                m_DeathPoolTimer += 1;
                m_DeathPool.Add(playerCar);

                UIManager.Current.SetCarPanel(playerCar, false);
                RemoveGameobjectFromTargetGroup(playerCar.gameObject);
            }
        }
    }

    private void ResetRace()
    {
        ResetCheckpoints();
        ResetCarLists();
        ResetCarHealth();
        ResetCarTransforms();
        ResetCarPanels();
        StartCountdown();
        ClearTargetGroup();
        AddObjectsToTargetGroup();
        PlayNextSong();
        PauseMusic();
    }

    private void ResetCarLists()
    {
        m_DeathPool.Clear();
        m_ActiveCars.Clear();
        m_ActiveCars.AddRange(m_AllCars);
    }

    private void ResetCarHealth()
    {
        foreach (var car in m_ActiveCars)
        {
            car.Health.ResetHealth();
        }
    }

    private void StartCountdown()
    {
        RaceTimer = 0f;
        RaceStatus = RaceStatus.Countdown;
        Countdown.StartCountdown();
    }

    private void ResetCarPanels()
    {
        foreach (var car in m_ActiveCars)
        {
            UIManager.Current.SetCarPanel(car, true);
        }
    }

    private void ResetCheckpoints()
    {
        RemoveCheckpointListener();
        FindStartCheckpoint();
        FindNextCheckpoint();
        AddCheckpointListener();
    }

    private void ResetCarTransforms()
    {
        if (m_StartCheckpoint == null)
        {
            Debug.LogError("No start checkpoint");
            return;
        }

        if (m_StartCheckpoint.TryGetComponent(out StartCheckpoint startCheckpoint))
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
                car.Health.CarHealthStatus -= OnCarStatusChanged;
            }
        }

        m_Countdown.CountdownEvent -= OnCountdownEvent;
    }

    private void OnCheckpointPassed(Checkpoint checkpointPassed)
    {
        RemoveCheckpointListener();
        m_LastCheckpoint = m_NextCheckpoint;
        
        if (checkpointPassed.gameObject.TryGetComponent(out StartCheckpoint startCheckpoint))
        {
            Debug.Log("new start checkpoint");
            m_StartCheckpoint = startCheckpoint;
        }
        else
        {
            Debug.Log("not startcheckpoin");
        }
        
        FindNextCheckpoint();
        AddCheckpointListener();
    }

    private void FindNextCheckpoint()
    {
        var checkpoints = TrackInformationManager.Current.Checkpoints;

        if (m_LastCheckpoint == null)
        {
            if (m_StartCheckpoint == null)
            {
                Debug.LogError("No Next Checkpoint");
            }
            else
            {
                m_LastCheckpoint = m_StartCheckpoint;
            }
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

    private void FindStartCheckpoint()
    {
        if (m_StartCheckpoint != null)
        {
            return;
        }

        var checkpoints = TrackInformationManager.Current.Checkpoints;

        for (var i = checkpoints.Length - 1; i >= 0; i--)
        {
            if (checkpoints[i].TryGetComponent(out StartCheckpoint _))
            {
                m_StartCheckpoint = checkpoints[i];
                return;
            }
        }

        Debug.LogError("No previous start checkpoint found");
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

    private void OnDestroy()
    {
        RemoveListeners();
    }
}