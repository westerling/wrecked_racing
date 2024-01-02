using UnityEngine;

public abstract class TargetWeapon : Weapon
{
    [SerializeField]
    private Transform m_LaserTransform;

    public Transform LaserTransform 
    {
        get => m_LaserTransform; 
    }
}
