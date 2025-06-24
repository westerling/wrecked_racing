using UnityEngine;

public class WeaponManager : CarComponent
{
    private Weapon m_Weapon;

    public Weapon Weapon
    {
        get => m_Weapon;
        private set => m_Weapon = value;
    }

    private void Start()
    {
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
            weaponToActivate.transform.position = Car.WeaponTransform.position;
            weaponToActivate.transform.rotation = Car.WeaponTransform.rotation;

            weapon.ParentTransform = transform;
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
        Weapon.ParentTransform = null;
        Weapon.gameObject.SetActive(false);
        Weapon.gameObject.transform.parent = WeaponPool.Current.transform;
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
            Car.Targeter.enabled = false;
        }

        Car.Targeter.enabled = Weapon is TargetWeapon;
    }

    private void OnFireStarted()
    {
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
