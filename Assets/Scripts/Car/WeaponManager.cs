using UnityEngine;

public class WeaponManager : CarComponent
{
    private Weapon m_Weapon;
    private PlayerCar m_PlayerCar;

    public Weapon Weapon
    {
        get => m_Weapon;
        private set => m_Weapon = value;
    }

    private void Start()
    {
        if (Car is PlayerCar playerCar)
        {
            m_PlayerCar = playerCar;
        }

        AddListeners();
        SetTargeterStatus();
    }

    public void AddWeapon(WeaponType weaponType)
    {
        var weaponToActivate = WeaponPool.Current.GetPooledObjectOfType(weaponType);

        if (weaponToActivate.TryGetComponent(out Weapon weapon))
        {
            weaponToActivate.SetActive(true);
            weaponToActivate.transform.parent = transform;
            weaponToActivate.transform.SetPositionAndRotation(
                m_PlayerCar.WeaponTransform.position,
                m_PlayerCar.WeaponTransform.rotation);

            weapon.PickupWeapon(Car);

            Weapon = weapon;
            Weapon.WeaponDepleated += OnWeaponDepleated;
            SetTargeterStatus();
        }
    }

    private void OnWeaponDepleated()
    {
        RemoveWeapon();
    }

    public void RemoveWeapon()
    {
        Weapon.ReleaseWeapon();
        
        if (WeaponPool.Current != null)
        {
            WeaponPool.Current.ReturnObjectToPool(Weapon.gameObject);
        }

        Weapon.WeaponDepleated -= OnWeaponDepleated;
        Weapon = null;
        SetTargeterStatus();
    }

    private void AddListeners()
    {
        Car.InputManager.FireStarted += OnFireStarted;
        Car.InputManager.FireStopped += OnFireCanceled;
    }

    private void RemoveListeners()
    {
        Car.InputManager.FireStarted -= OnFireStarted;
        Car.InputManager.FireStopped -= OnFireCanceled;

        if (Weapon != null)
        {
            RemoveWeapon();
        }
    }

    private void SetTargeterStatus()
    {
        if (Weapon == null)
        {
            m_PlayerCar.Targeter.enabled = false;
        }

        m_PlayerCar.Targeter.enabled = Weapon is TargetWeapon;
    }

    private void OnFireStarted()
    {
        if (RaceManager.Current.RaceStatus != RaceStatus.Race)
        {
            return;
        }

        if (Weapon == null)
        {
            return;
        }

        Weapon.IsFiring = true;
    }

    private void OnFireCanceled()
    {
        if (Weapon == null)
        {
            return;
        }

        Weapon.IsFiring = false;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
