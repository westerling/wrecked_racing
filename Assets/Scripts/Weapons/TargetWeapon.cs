using UnityEngine;

public abstract class TargetWeapon : Weapon
{
    [SerializeField]
    private Transform m_LaserTransform;

    [SerializeField]
    private GameObject m_MovingObject;

    public Transform LaserTransform 
    {
        get => m_LaserTransform; 
    }

    public GameObject MovingObject
    {
        get => m_MovingObject;
    }
}
