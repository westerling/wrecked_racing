using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField]
    private Elevator m_Elevator;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.attachedRigidbody.TryGetComponent(out PlayerCar car))
        {
            m_Elevator.CarEntered(car);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.attachedRigidbody.TryGetComponent(out PlayerCar car))
        {
            m_Elevator.CarExited(car);
        }
    }
}
