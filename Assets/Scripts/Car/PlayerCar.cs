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

    protected override void Awake()
    {
        base.Awake();

        AddListeners();

        IsAi = false;
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
        m_Health.CarActive += OnCarActive;
    }

    private void RemoveListeners()
    {
        m_Health.CarActive -= OnCarActive;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
