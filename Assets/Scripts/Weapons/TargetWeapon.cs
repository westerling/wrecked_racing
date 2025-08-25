using UnityEngine;

public abstract class TargetWeapon : Weapon
{
    [SerializeField]
    private Transform m_LaserOrigin;

    [SerializeField]
    private GameObject m_MovingObject;

    public Transform LaserTransform 
    {
        get => m_LaserOrigin; 
    }

    public GameObject MovingObject
    {
        get => m_MovingObject;
    }
}
