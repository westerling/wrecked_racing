using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Car : MonoBehaviour
{
    public event Action<Car, bool> CarActive;

    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private Transform m_CenterOfMass;

    [SerializeField]
    private CarStatsManager m_StatusManager;

    [SerializeField]
    private Health m_Health;

    [SerializeField]
    private Stats m_Stats;

    [SerializeField]
    private Wheel[] m_Wheels;

    private List<Wheel> m_DriveWheels = new List<Wheel>();
    private List<Wheel> m_SteeringWheels = new List<Wheel>();
    
    private Player m_Player;
    private InputManager m_InputManager;

    private float m_CurrentSpeedRatio = 0f;
    private float m_CurrentSpeed = 0f;

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

    public Wheel[] Wheels
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
    
    public Player Player 
    {
        get => m_Player; 
        set => m_Player = value; 
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
    
    public List<Wheel> DriveWheels 
    { 
        get => m_DriveWheels; 
        private set => m_DriveWheels = value; 
    }

    public List<Wheel> SteeringWheels
    {
        get => m_SteeringWheels;
        private set => m_SteeringWheels = value;
    }

    public CarDirection HeadingDirection
    {
        get => m_HeadingDirection;
        private set => m_HeadingDirection = value;
    }

    public void ResetCar()
    {
        foreach (var wheel in m_Wheels)
        {
            wheel.MotorTorque = 0f;
        }

        Rigidbody.angularVelocity = Vector3.zero;
    }

    private void Awake()
    {
        Rigidbody.centerOfMass = m_CenterOfMass.localPosition;

        SetDriveWheels();
        SetSteeringWheels();
    }

    private void SetDriveWheels()
    {
        switch (Stats.DriveTrain)
        {
            case Drivetrain.FWD:
                DriveWheels.AddRange(Wheels.Where(
                    x => x.WheelPlacement == WheelPlacement.FrontLeft ||
                    x.WheelPlacement == WheelPlacement.FrontRight));
                break;
            case Drivetrain.RWD:
                DriveWheels.AddRange(Wheels.Where(
                    x => x.WheelPlacement == WheelPlacement.RearLeft ||
                    x.WheelPlacement == WheelPlacement.RearRight));
                break;
            case Drivetrain.AWD:
                DriveWheels.AddRange(Wheels);
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        AddListeners();
    }

    private void SetSteeringWheels()
    {
        SteeringWheels.AddRange(Wheels.Where(
            x => x.WheelPlacement == WheelPlacement.FrontLeft ||
            x.WheelPlacement == WheelPlacement.FrontRight));
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

    private void OnCarActive(bool active)
    {
        CarActive?.Invoke(this, active);
    }

    private void AddListeners()
    {
        m_Health.CarActive += OnCarActive;
    }

    private void RemoveListeners()
    {
        m_Health.CarActive -= OnCarActive;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
