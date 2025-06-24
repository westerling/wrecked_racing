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

    private Transform m_ParentTransform;
    
    protected float m_LastFireTime = -Mathf.Infinity;

    public Transform ParentTransform
    {
        get => m_ParentTransform;
        set => m_ParentTransform = value;
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

    protected abstract void Fire();

    protected virtual void OnEnable()
    {
        m_Ammunition = m_StartAmmunition;
    }

    protected virtual void Update()
    {
        CheckFiring();
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
        m_Ammunition--;

        if (m_Ammunition <= 0)
        {
            IsFiring = false;
            WeaponDepleated?.Invoke();
            return;
        }
    }
}
