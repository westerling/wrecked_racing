using System.Linq;
using UnityEngine;

public class PlayerCar : Car
{
    [SerializeField]
    private Health m_Health;

    [SerializeField]
    private Transform m_WeaponTransform;

    [SerializeField]
    private WeaponManager m_WeaponManager;

    [SerializeField]
    private Targeter m_Targeter;

    [SerializeField]
    private CarVisualsBase[] m_CarVisualBases;

    private Player m_Player;

    private bool m_RaceActive = false;
    private float m_Timer;

    public WeaponManager WeaponManager
    {
        get => m_WeaponManager;
    }

    public Transform WeaponTransform
    {
        get => m_WeaponTransform;
    }

    public Targeter Targeter
    {
        get => m_Targeter;
    }

    public Player Player
    {
        get => m_Player;
        set => m_Player = value;
    }

    public Health Health
    {
        get => m_Health;
    }

    protected override void Awake()
    {
        base.Awake();

        AddListeners();

        Debug.Log("Get player");

        IsAi = false;
    }

    private void Start()
    {
        var carVisualBase = m_CarVisualBases.Where(x => x.Color == Player.Color).FirstOrDefault();

        if (carVisualBase == null)
        {
            Debug.LogError("Visuals not found for color " + Player.Color);
        }

        carVisualBase.gameObject.SetActive(true);
    }

    private void Update()
    {
        CheckStationaryCar();
    }

    private void CheckStationaryCar()
    {
        if (!m_RaceActive)
        {
            return;
        }
        if (CurrentSpeed < 1)
        {
            m_Timer += Time.deltaTime;

            if (m_Timer >= 3)
            {
                Health.Damage(float.MaxValue);
            }
        }
        else
        {
            m_Timer = 0f;
        }
    }

    private void OnCarActive(CarStatus carStatus)
    {
        if (carStatus == CarStatus)
        {
            return;
        }

        CarStatus = carStatus;
    }

    private void AddListeners()
    {
        Health.CarHealthStatus += OnCarActive;
        RaceManager.Current.RaceStatusChanged += OnRaceStatusChanged;
    }

    private void OnRaceStatusChanged(RaceStatus raceStatus)
    {
        m_Timer = 0;
        switch (raceStatus)
        {
            case RaceStatus.Race:
                m_RaceActive = true;
                break;
            default:
                m_RaceActive = false;
                break;
        }
    }

    private void RemoveListeners()
    {
        Health.CarHealthStatus -= OnCarActive;
        RaceManager.Current.RaceStatusChanged -= OnRaceStatusChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
