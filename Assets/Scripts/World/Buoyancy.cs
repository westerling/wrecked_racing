using UnityEngine;

public class Buoyancy : MonoBehaviour
{
    private float buoyancyForce = 10f;

    private Rigidbody m_RigidBody;

    void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        m_RigidBody.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
    }
}
