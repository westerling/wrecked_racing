using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    WeaponType m_WeaponType;

    [SerializeField]
    private int m_Ammunition;

    [SerializeField]
    private bool m_IsAutomatic;

    public WeaponType WeaponType 
    {
        get => m_WeaponType; 
    }

    public abstract void Fire();
}
