using System;
using Unity.Mathematics;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    [SerializeField]
    private Transform m_ParentTransform;

    [SerializeField]
    private float m_Threshold;

    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private Collider m_Collider;

    private bool m_Detached;

    private Vector3 m_StartPos;
    private quaternion m_StartRotation;

    public bool Detached
    {
        get => m_Detached;
        set => m_Detached = value;
    }

    public float Threshold
    {
        get => m_Threshold;
    }

    private void Start()
    {
        SeperateComponent(false);
        //SetStartPosition();
    }

    private void SetStartPosition()
    {
        m_StartPos = transform.position;
        m_StartRotation = transform.rotation;
    }

    public void SeperateComponent(bool enable)
    {
        m_Collider.enabled = enable;
        m_Rigidbody.isKinematic = !enable;
        Detached = enable;
        transform.parent = enable ? null : m_ParentTransform;
    }

    public void ResetTransformAndPosition()
    {
        //transform.SetPositionAndRotation(m_StartPos, m_StartRotation);
        SeperateComponent(true);
    }
}
