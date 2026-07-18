using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private PhysicsMaterial m_PhysicsMaterial;

    [SerializeField]
    private Transform m_CenterOfMass;

    [SerializeField]
    private CarStatsManager m_StatusManager;

    [SerializeField]
    private Transmission m_Transmission;

    [SerializeField]
    private Stats m_Stats;

    [SerializeField]
    private Health m_Health;

    private float m_CurrentSpeedRatio = 0f;
    private float m_CurrentSpeed = 0f;
    private bool m_IsAi;

    private List<Wheel> m_Wheels = new List<Wheel>();
    private InputManager m_InputManager;

    private CarDirection m_HeadingDirection = CarDirection.Stationary;


    public Rigidbody Rigidbody
    {
        get => m_Rigidbody;
    }

    public Transform CenterOfMass
    {
        get => m_CenterOfMass;
    }

    public InputManager InputManager
    {
        get => m_InputManager;
        set => m_InputManager = value;
    }

    public List<Wheel> Wheels
    {
        get => m_Wheels;
    }

    public CarStatsManager StatusManager
    { 
        get => m_StatusManager;
    }

    public Stats Stats 
    { 
        get => m_Stats; 
    }
    
    public float CurrentSpeedRatio 
    {
        get => m_CurrentSpeedRatio; 
        private set => m_CurrentSpeedRatio = value; 
    }

    public float CurrentSpeed 
    { 
        get => m_CurrentSpeed; 
        private set => m_CurrentSpeed = value;
    }
    
    public CarDirection HeadingDirection
    {
        get => m_HeadingDirection;
        private set => m_HeadingDirection = value;
    }

    public Transmission Transmission
    {
        get => m_Transmission;
    }
    
    public bool IsAi
    {
        get => m_IsAi;
        protected set => m_IsAi = value;
    }

    public Health Health
    {
        get => m_Health;
    }

    public PhysicsMaterial PhysicsMaterial
    {
        get => m_PhysicsMaterial;
    }

    protected virtual void Awake()
    {
        Rigidbody.centerOfMass = m_CenterOfMass.localPosition;

        GetWheels();
    }

    private void GetWheels()
    {
        Wheels.Add(Transmission.FrontDifferential.LeftWheel);
        Wheels.Add(Transmission.FrontDifferential.RightWheel);
        Wheels.Add(Transmission.RearDifferential.LeftWheel);
        Wheels.Add(Transmission.RearDifferential.RightWheel);
    }

    public void SetCarStationary(Vector3 position, Quaternion rotation)
    {
        foreach (var wheel in Wheels)
        {
            wheel.MotorTorque = 0f;
        }

        transform.SetPositionAndRotation(position, rotation);

        Rigidbody.angularVelocity = Vector3.zero;
        Rigidbody.linearVelocity = Vector3.zero;
        Rigidbody.position = position;
        Rigidbody.rotation = rotation;
        Rigidbody.Sleep();
    }

    private void FixedUpdate()
    {
        CalculateCarVelocity();
        SetCarDirection();
    }

    private void SetCarDirection()
    {
        if (CurrentSpeed > 1)
        {
            m_HeadingDirection = CarDirection.Forward;
        }
        else if (CurrentSpeed < -1)
        {
            m_HeadingDirection = CarDirection.Backward;
        }
        else
        {
            m_HeadingDirection = CarDirection.Stationary;
        }
    }

    private void CalculateCarVelocity()
    {
        CurrentSpeed = transform.InverseTransformDirection(Rigidbody.linearVelocity).z;
        CurrentSpeedRatio = CurrentSpeed / Stats.TopSpeed;
    }
}
