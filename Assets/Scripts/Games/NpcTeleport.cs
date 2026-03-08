using UnityEngine;

public class NpcTeleport : MonoBehaviour
{
    [SerializeField]
    private Transform m_TeleportTransform;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out NpcCar npcCar))
        {
            npcCar.transform.position = m_TeleportTransform.position;

            var rigidBody = other.attachedRigidbody;
            if (rigidBody != null)
            {
                rigidBody.linearVelocity = Vector3.zero;
                rigidBody.angularVelocity = Vector3.zero;
            }

            npcCar.transform.SetPositionAndRotation(
                m_TeleportTransform.position,
                m_TeleportTransform.rotation
            );
        }
    }
}
