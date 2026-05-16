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

        if (car is PlayerCar playerCar)
        {
            playerCar.Health.Damage(float.MaxValue);
        }
    }
}
