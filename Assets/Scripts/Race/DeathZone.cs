using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var car = other.GetComponentInParent<Car>();

        if (car == null)
        {
            return;
        }

        if (car.gameObject.TryGetComponent(out Health health))
        {
            health.Destroy();
        }
    }
}
