using System;
using System.Linq;
using UnityEngine;

public class Steering : CarComponent
{
    private float m_WheelBase;
    private float m_RearTrack;
    private float m_SteerInput;
    private float m_SteerAngleLeft;
    private float m_SteerAngleRight;

    private Wheel m_LeftSteeringWheel;
    private Wheel m_RightSteeringWheel;

    private void Start()
    {
        Car.InputManager.Steer += OnSteerPerformed;

        SetSteeringWheels();
        SetWheelBase();
        SetRearTrack();
    }

    private void Update()
    {
        if (m_SteerInput > 0)
        {
            var steerAngleLeft = Mathf.Rad2Deg * Mathf.Atan(m_WheelBase / (Car.Stats.TurnRadius + (m_RearTrack / 2))) * m_SteerInput;
            var steerAngleRight = Mathf.Rad2Deg * Mathf.Atan(m_WheelBase / (Car.Stats.TurnRadius - (m_RearTrack / 2))) * m_SteerInput;

            m_SteerAngleLeft = UpdateSteerAngle(m_SteerAngleLeft, steerAngleLeft);
            m_SteerAngleRight = UpdateSteerAngle(m_SteerAngleRight, steerAngleRight);
            UpdateVisuals();
            return;
        }

        if (m_SteerInput < 0)
        {
            var steerAngleLeft = Mathf.Rad2Deg * Mathf.Atan(m_WheelBase / (Car.Stats.TurnRadius + (m_RearTrack / 2))) * m_SteerInput;
            var steerAngleRight = Mathf.Rad2Deg * Mathf.Atan(m_WheelBase / (Car.Stats.TurnRadius - (m_RearTrack / 2))) * m_SteerInput;

            m_SteerAngleLeft = UpdateSteerAngle(m_SteerAngleLeft, steerAngleLeft);
            m_SteerAngleRight = UpdateSteerAngle(m_SteerAngleRight, steerAngleRight);

            UpdateVisuals();
            return;
        }

        m_SteerAngleLeft = 0f;
        m_SteerAngleRight = 0f;
        UpdateVisuals();
    }

    private float UpdateSteerAngle(float currentSteerAngle, float targetSteerAngle)
    {
        return Mathf.MoveTowards(currentSteerAngle, targetSteerAngle, Time.deltaTime * Car.Stats.SteerAngleChangeSpeed);
    }

    private void UpdateVisuals()
    {
        m_LeftSteeringWheel.SteerAngle = m_SteerAngleLeft;
        m_RightSteeringWheel.SteerAngle = m_SteerAngleRight;
    }

    private void OnSteerPerformed(float obj)
    {
        m_SteerInput = obj;
    }

    private void SetRearTrack()
    {
        var leftWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.RearLeft).FirstOrDefault();
        var rightWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.RearRight).FirstOrDefault();

        if (leftWheel != null && rightWheel != null)
        {
            m_RearTrack = Math.Abs(leftWheel.transform.localPosition.x - rightWheel.transform.localPosition.x);
        }

        m_RearTrack = 2f;
    }

    private void SetWheelBase()
    {
        var frontLeftWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.FrontLeft).FirstOrDefault();
        var frontRightWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.FrontRight).FirstOrDefault();
        var rearLeftWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.RearLeft).FirstOrDefault();
        var rearRightWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.RearRight).FirstOrDefault();

        if (frontLeftWheel != null && frontRightWheel != null && rearLeftWheel != null && rearRightWheel != null)
        {
            m_WheelBase = Math.Abs(((
                frontLeftWheel.transform.localPosition.y + frontRightWheel.transform.localPosition.y) / 2) 
                - ((rearLeftWheel.transform.localPosition.y + rearRightWheel.transform.localPosition.y) / 2));
        }

        m_WheelBase = 4f;
    }

    private void SetSteeringWheels()
    {
        m_LeftSteeringWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.FrontLeft).FirstOrDefault();
        m_RightSteeringWheel = Car.Wheels.Where(x => x.WheelPlacement == WheelPlacement.FrontRight).FirstOrDefault();
    }

    private void OnDestroy()
    {
        Car.InputManager.Steer -= OnSteerPerformed;
    }
}
