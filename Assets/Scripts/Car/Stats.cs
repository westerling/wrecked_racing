using System;
using UnityEngine;

[Serializable]
public class Stats
{
    [Header("Information")]
    [SerializeField]
    private string m_Name;

    [SerializeField]
    private Sprite m_Image;

    [SerializeField]
    private string m_Description;

    [SerializeField]
    private Drivetrain m_DriveTrain;

    [SerializeField]
    private float m_TopSpeed = 100f;

    [SerializeField]
    private AnimationCurve m_MotorTorque;

    [Header("Brakes")]
    [SerializeField]
    private AnimationCurve m_BrakeStrength;

    [Header("Handling")]
    [SerializeField]
    private AnimationCurve m_TurningCurve;

    [Header("Handling")]
    [SerializeField]
    private AnimationCurve m_SlipCurveFront;

    [Header("Handling")]
    [SerializeField]
    private AnimationCurve m_SlipCurveRear;

    [Header("Chassi")]
    [SerializeField]
    [Range(0f, 10f)]
    private float m_DragCoefficient = 1f;

    [Range(0f, 100f)]
    [SerializeField]
    private float m_HealthPoints = 50f;

    [SerializeField]
    [Range(0f, 5f)]
    private float m_Downforce = 2f;

    [SerializeField]
    private CarPartStats[] m_CarPartStats;

    [Header("Audio")]
    [SerializeField]
    private AudioSource m_EngineSound;

    public float TopSpeed 
    {
        get => m_TopSpeed; 
    }

    public AnimationCurve BrakeStrength
    {
        get => m_BrakeStrength; 
    }
        
    public AnimationCurve TurningCurve 
    {
        get => m_TurningCurve; 
    }

    public float DragCoefficient 
    {
        get => m_DragCoefficient; 
    }
    public string Name
    {
        get => m_Name; 
    }

    public Drivetrain DriveTrain 
    {
        get => m_DriveTrain; 
    }
    
    public float HealthPoints 
    {
        get => m_HealthPoints; 
    }

    public float Downforce 
    {
        get => m_Downforce; 
    }

    public AnimationCurve MotorTorque
    {
        get => m_MotorTorque;
    }

    public AnimationCurve SlipCurveFront
    {
        get => m_SlipCurveFront;
    }

    public AnimationCurve SlipCurveRear
    {
        get => m_SlipCurveRear;
    }

    public AudioSource EngineSound
    {
        get => m_EngineSound;
    }
    
    public string Description
    {
        get => m_Description;
    }

    public Sprite Image
    {
        get => m_Image;
    }

    public CarPartStats[] CarPartStats
    {
        get => m_CarPartStats;
    }
}
