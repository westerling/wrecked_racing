using UnityEngine;

public class Downforce : CarComponent
{
    void Update()
    {
        var speed = Vector3.Dot(Car.transform.forward, Car.Rigidbody.velocity);
        var lift = Car.Stats.Downforce * speed;
        Car.Rigidbody.AddForceAtPosition(lift * -transform.up, transform.position);
    }
}
