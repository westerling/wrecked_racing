using System;
using UnityEngine;

[Serializable]
public class Stats
{
    [Header("Information")]
    [SerializeField]
    private string m_Name;

    [SerializeField]
    private Drivetrain m_DriveTrain;

    [Header("Engine")]
    [SerializeField]
    private AnimationCurve m_Torque;

    [SerializeField]
    [Range(5f, 20f)]
    private float m_TopSpeed = 12f;

    [SerializeField]
    [Range(50f, 1000f)]
    private float m_HorsePower = 136f;

    [Header("Brakes")]
    [SerializeField]
    [Range(100f, 400f)]
    private float m_Brakes= 250f;

    [Header("Steering")]
    [SerializeField]
    [Range(50f, 100f)]
    private float m_SteerAngleChangeSpeed = 75f;

    [SerializeField]
    [Range(5f, 20f)]
    private float m_TurnRadius = 10f;

    [Header("Tires")]
    [SerializeField]
    [Range(5f, 20f)]
    private float m_TireMass = 10f;

    [SerializeField]
    private AnimationCurve m_FrontTireGrip;

    [SerializeField]
    private AnimationCurve m_RearTireGrip;

    [Header("Suspension")]
    [SerializeField]
    [Range(0f, 2f)]
    private float m_SuspensionRestDistance = 1.5f;

    [SerializeField]
    private float m_SpringStrength = 5000f;

    [SerializeField]
    private float m_SpringDamper = 800f;

    [Header("Body")]
    [SerializeField]
    [Range(1f, 10f)]
    private float m_Downforce = 5f;

    [SerializeField]
    [Range(1f, 100f)]
    private float m_SlipstreamEffect = 5f;

    public float SuspensionRestDistance 
    {
        get => m_SuspensionRestDistance;
    }

    public float SpringStrength 
    {
        get => m_SpringStrength;
    }

    public float SpringDamper
    {
        get => m_SpringDamper; 
    }

    public AnimationCurve FrontTireGrip 
    {
        get => m_FrontTireGrip;
    }

    public AnimationCurve RearTireGrip 
    {
        get => m_RearTireGrip; 
    }

    public float TireMass
    {
        get => m_TireMass; 
    }

    public AnimationCurve Torque 
    { 
        get => m_Torque; 
    }

    public float TopSpeed 
    { 
        get => m_TopSpeed;
    }

    public float SteerAngleChangeSpeed 
    {
        get => m_SteerAngleChangeSpeed; 
    }
    public float TurnRadius 
    {
        get => m_TurnRadius; 
    }

    public Drivetrain DriveTrain 
    {
        get => m_DriveTrain; 
    }

    public float HorsePower 
    {
        get => m_HorsePower;
    }

    public float Downforce 
    { 
        get => m_Downforce; 
    }

    public float Brakes 
    {
        get => m_Brakes; 
    }

    public float SlipstreamEffect 
    {
        get => m_SlipstreamEffect; 
    }
}
