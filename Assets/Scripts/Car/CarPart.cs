using System;
using System.Linq;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    [SerializeField]
    private CarPartType m_CarPartType;

    private Vector3 m_StartPosition;
    private Quaternion m_StartRotation;
    private Vector3 m_StartScale;

    private bool m_Detached;
    private float m_Threshold;

    private Collider m_Collider;
    private Rigidbody m_Rigidbody;
    private Transform m_ParentTransform;

    public bool Detached
    {
        get => m_Detached;
        set => m_Detached = value;
    }

    public float Threshold
    {
        get => m_Threshold;
        private set => m_Threshold = value;
    }

    public void SetupCarPart(Transform parentObject, Car car)
    {
        SetStartTransform();

        var stats = car.Stats.CarPartStats.FirstOrDefault(x => x.CarPartType == m_CarPartType);

        if (stats == null)
        {
            Debug.LogError("No Stats found for car part " + m_CarPartType);
        }

        m_ParentTransform = parentObject;

        var meshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        meshCollider.convex = true;
        meshCollider.material = car.PhysicsMaterial;
        m_Collider = meshCollider;

        var rigidBody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        
        if (stats == null)
        {
            Debug.LogError($"No stats found for car part {m_CarPartType}");
            enabled = false;
            return;
        }

        rigidBody.mass = stats.Mass;
        m_Rigidbody = rigidBody;

        Threshold = stats.Threshold;

        DetachComponent(false);
    }

    private void SetStartTransform()
    {
        m_StartPosition = transform.localPosition;
        m_StartRotation = transform.localRotation;
        m_StartScale = transform.localScale;
    }

    public void DetachComponent(bool detached)
    {
        m_Collider.enabled = detached;
        m_Rigidbody.isKinematic = !detached;
        Detached = detached;
        transform.SetParent(detached ? null : m_ParentTransform);

        if (!detached)
        {
            transform.localPosition = m_StartPosition;
            transform.localRotation = m_StartRotation;
            transform.localScale = m_StartScale;
        }
    }

    public void ResetTransformAndPosition()
    {
        DetachComponent(false);
    }
}
