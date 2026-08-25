using UnityEngine;

public class CarPart : MonoBehaviour
{
    [Range(0.01f, 1f)]
    [SerializeField]
    private float m_Threshold;

    [Range(1f, 100f)]
    [SerializeField]
    private float m_Mass;

    private Vector3 m_StartPosition;
    private Quaternion m_StartRotation;
    private Vector3 m_StartScale;

    private bool m_Detached;

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

        m_ParentTransform = parentObject;

        if (gameObject.TryGetComponent(out MeshCollider collider))
        {
            m_Collider = collider;
        }

        var rigidBody = gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;

        rigidBody.mass = m_Mass;
        m_Rigidbody = rigidBody;

        Threshold = m_Threshold;

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
