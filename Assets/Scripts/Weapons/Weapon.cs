using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public event Action WeaponDepleated;

    [SerializeField]
    WeaponType m_WeaponType;

    [SerializeField]
    private float m_FireRate;
    
    [SerializeField]
    private int m_StartAmmunition;

    [SerializeField]
    private bool m_IsAutomatic;

    private bool m_IsFiring = false;
    private int m_Ammunition;

    private Car m_ParentCar;

    protected float m_LastFireTime = -Mathf.Infinity;

    protected Car ParentCar
    {
        get => m_ParentCar;
        private set => m_ParentCar = value; 
    }

    public WeaponType WeaponType 
    {
        get => m_WeaponType; 
        protected set => m_WeaponType = value;
    }

    public bool IsFiring
    {
        get => m_IsFiring;
        set => m_IsFiring = value;
    }

    protected int Ammunition
    {
        get => m_Ammunition;
        private set => m_Ammunition = value;
    }

    public virtual void PickupWeapon(Car car)
    {
        ParentCar = car;
    }

    public virtual void ReleaseWeapon()
    {
        ParentCar = null;
    }

    protected abstract void Fire();

    protected virtual void OnEnable()
    {
        Ammunition = m_StartAmmunition;
    }

    protected virtual void Update()
    {
        CheckFiring();
    }

    protected virtual void ReturnWeapon()
    {
    }

    private void CheckFiring()
    {
        if (IsFiring && Time.time >= m_LastFireTime + m_FireRate)
        {
            Fire();
            CheckAmmunition();
            SetFiring();
        }
    }

    private void SetFiring()
    {
        m_LastFireTime = Time.time;

        if (!m_IsAutomatic)
        {
            IsFiring = false;
        }
    }

    private void CheckAmmunition()
    {
        Ammunition--;

        if (Ammunition <= 0)
        {
            IsFiring = false;
            ReturnWeapon();
            WeaponDepleated?.Invoke();
            return;
        }
    }
}
