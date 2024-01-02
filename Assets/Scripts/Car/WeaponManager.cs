using System;
using System.Linq;
using UnityEngine;

public class WeaponManager : CarComponent
{
    private Weapon m_Weapon;

    [SerializeField]
    private GameObject[] m_Weapons;

    private void Start()
    {
        AddListeners();
    }

    public bool HasWeapon()
    {
        return m_Weapon != null;
    }

    public void AddWeapon(WeaponType weaponType)
    {
        var weaponToActivate = m_Weapons.Where(x => x.GetComponent<Weapon>().WeaponType == weaponType).FirstOrDefault();
    }

    private void AddListeners()
    {
        Car.InputManager.Fire += FirePerformed;
    }

    private void RemoveListeners()
    {
        Car.InputManager.Fire -= FirePerformed;
    }

    private void FirePerformed()
    {
        if (m_Weapon == null)
        {
            return;
        }

        m_Weapon.Fire();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
