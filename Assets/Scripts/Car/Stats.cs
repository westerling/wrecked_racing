using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Stats
{
    [Header("Information")]
    [SerializeField]
    private string m_Name;

    [SerializeField]
    private Drivetrain m_DriveTrain;

    [SerializeField]
    private float m_TopSpeed = 100f;

    [SerializeField]
    private AnimationCurve m_MotorTorque;

    [Header("Brakes")]
    [SerializeField]
    private AnimationCurve m_BrakeStrength;

    [Header("Steering")]
    [SerializeField]
    private AnimationCurve m_TurningCurve;

    [Header("Chassi")]
    [SerializeField]
    [Range(0f, 10f)]
    private float m_DragCoefficient = 1f;

    [Range(0f, 100f)]
    [SerializeField]
    private float m_Health = 50f;

    [SerializeField]
    [Range(0f, 5f)]
    private float m_Downforce = 2f;

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
    
    public float Health 
    {
        get => m_Health; 
    }

    public float Downforce 
    {
        get => m_Downforce; 
    }

    public AnimationCurve MotorTorque
    {
        get => m_MotorTorque;
    }
}
