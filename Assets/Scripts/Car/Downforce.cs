public class Downforce : CarComponent
{
    private float m_DownForce;

    private void Start()
    {
        m_DownForce = Car.Stats.Downforce;
    }

    void FixedUpdate()
    {
        var lift = m_DownForce * Car.CurrentSpeedRatio;
        Car.Rigidbody.AddForceAtPosition(lift * -transform.up, Car.CenterOfMass.position);
    }
}
