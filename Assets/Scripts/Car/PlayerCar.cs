using UnityEngine;

public class PlayerCar : Car
{
    [SerializeField]
    private Transform m_WeaponTransform;

    [SerializeField]
    private WeaponManager m_WeaponManager;

    [SerializeField]
    private Targeter m_Targeter;

    [SerializeField]
    private MeshRenderer[] m_ColorRenderers;

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

    protected override void Awake()
    {
        base.Awake();

        AddListeners();

        IsAi = false;
    }

    private void Start()
    {
        SetMaterial();
    }

    private void SetMaterial()
    {
        var newMaterial = ColorManager.Current.GetMaterial(m_Player.Color);

        foreach (var colorRenderer in m_ColorRenderers)
        {
            colorRenderer.material = newMaterial;
        }
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

            if (m_Timer >= 5)
            {
                Health.Damage(float.MaxValue);
                m_Timer = 0;
            }
        }
        else
        {
            m_Timer = 0f;
        }
    }

    private void AddListeners()
    {
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
        RaceManager.Current.RaceStatusChanged -= OnRaceStatusChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
