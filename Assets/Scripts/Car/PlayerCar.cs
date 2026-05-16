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
    }

    private void RemoveListeners()
    {
        Health.CarHealthStatus -= OnCarActive;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
