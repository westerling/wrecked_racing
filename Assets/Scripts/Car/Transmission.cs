using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmission : CarComponent
{
    [SerializeField]
    private Differential frontDifferential;

    [SerializeField]
    private Differential rearDifferential;

    private float m_MotorTorque;
    private int m_CurrentGear = 0;
    private int m_Gears = 5;
    private float m_FinalDriveRatio = 3.9f;
    private float m_MaxGearRatio = 3.5f;
    private float m_MinGearRatio = 0.8f;
    private float m_CenterSplit = 0.5f;
    private bool m_SwitchGear = false;
    private List<float> m_GearRatios = new List<float>();

    public float MotorTorque
    {
        get => m_MotorTorque;
        set => m_MotorTorque = value;
    }

    public Differential FrontDifferential
    {
        get => frontDifferential;
    }

    public Differential RearDifferential
    {
        get => rearDifferential;
    }

    private void Start()
    {
        InitializeGears();
    }

    private void Update()
    {
        SetGear();
        ApplyMotorTorque();
    }

    private void InitializeGears()
    {
        m_GearRatios.Clear();

        for (var i = 0; i < m_Gears; i++)
        {
            var t = (float)i / (m_Gears - 1);
            var ratio = Mathf.Lerp(m_MaxGearRatio, m_MinGearRatio, t);
            m_GearRatios.Add(ratio);
        }
    }

    private void SetGear()
    {
        var gearBandSize = 1f / m_Gears;
        var newGear = Mathf.FloorToInt(Car.CurrentSpeedRatio / gearBandSize);
        var nextGear = Mathf.Clamp(newGear, 0, m_Gears - 1);

        if (nextGear != m_CurrentGear && !m_SwitchGear)
        {
            StartCoroutine(SwitchGear(nextGear));
        }
    }

    private IEnumerator SwitchGear(int nextGear)
    {
        m_SwitchGear = true;
        yield return new WaitForSeconds(0.4f);
        m_SwitchGear = false;

        m_CurrentGear = nextGear;
    }

    private void ApplyMotorTorque()
    {
        var torque = MotorTorque * m_GearRatios[m_CurrentGear] * m_FinalDriveRatio * (m_SwitchGear ? 0.5f : 1f);

        switch (Car.Stats.DriveTrain)
        {
            case Drivetrain.FWD:
                FrontDifferential.ApplyTorque(torque);
                break;

            case Drivetrain.RWD:
                RearDifferential.ApplyTorque(torque);
                break;

            case Drivetrain.AWD:
                FrontDifferential.ApplyTorque(torque * m_CenterSplit);
                RearDifferential.ApplyTorque(torque * (1 - m_CenterSplit));
                break;
        }
    }
}
