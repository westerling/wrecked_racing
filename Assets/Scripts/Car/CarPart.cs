using System.Linq;
using UnityEngine;

public class CarPart : MonoBehaviour
{
    [SerializeField]
    private CarPartType m_CarPartType;

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

    public void SetupCarPart(Car car)
    {
        var stats = car.Stats.CarPartStats.Where(x => x.CarPartType == m_CarPartType).FirstOrDefault();

        if (stats == null)
        {
            Debug.LogError("No Stats found for car part " + m_CarPartType);
        }

        m_ParentTransform = car.transform;

        var meshCollider = gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        meshCollider.convex = true;
        meshCollider.material = car.PhysicsMaterial;
        m_Collider = meshCollider;

        var rigidBody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
        rigidBody.mass = stats.Mass;
        m_Rigidbody = rigidBody;

        Threshold = stats.Threshold;

        SeperateComponent(false);
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
        SeperateComponent(true);
    }
}
