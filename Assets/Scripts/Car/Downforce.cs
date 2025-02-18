public class Downforce : CarComponent
{
    void FixedUpdate()
    {
        var lift = Car.Stats.Downforce * Car.CurrentSpeedRatio;
        Car.Rigidbody.AddForceAtPosition(lift * -transform.up, Car.CenterOfMass.position);
    }
}
