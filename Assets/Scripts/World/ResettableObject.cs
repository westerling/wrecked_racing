using UnityEngine;

public class ResettableObject : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_RigidBody;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 startScale;

    private void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        startScale = transform.localScale;
    }

    private void Start()
    {
        RaceManager.Current.RaceStatusChanged += OnRaceStatusChanged;
    }

    private void OnRaceStatusChanged(RaceStatus status)
    {
        if (status == RaceStatus.Countdown)
        {
            ResetObject();
        }
    }

    private void ResetObject()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;

        if (m_RigidBody != null)
        {
            m_RigidBody.linearVelocity = Vector3.zero;
            m_RigidBody.angularVelocity = Vector3.zero;
        }
    }

    private void OnDestroy()
    {
        RaceManager.Current.RaceStatusChanged -= OnRaceStatusChanged;
    }
}
