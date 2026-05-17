using System;
using System.Linq;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    [SerializeField]
    private CarPartType m_CarPartType;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

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

        var stats = car.Stats.CarPartStats.Where(x => x.CarPartType == m_CarPartType).FirstOrDefault();

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
        rigidBody.mass = stats.Mass;
        m_Rigidbody = rigidBody;

        Threshold = stats.Threshold;

        DetachComponent(false);
    }

    private void SetStartTransform()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
    }

    public void DetachComponent(bool detached)
    {
        m_Collider.enabled = detached;
        m_Rigidbody.isKinematic = !detached;
        Detached = detached;
        transform.parent = detached ? null : m_ParentTransform;

        if (!detached)
        {
            transform.position = startPosition;
            transform.rotation = startRotation;
            transform.localScale = startScale;
        }
    }

    public void ResetTransformAndPosition()
    {
        DetachComponent(false);
    }
}
