using Unity.Cinemachine;
using System;
using UnityEngine;
using static Unity.Cinemachine.CinemachineBlendDefinition;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public event Action<CinemachineTargetGroup> TargetGroupChanged;
    public event Action<GameObject> TargetChanged;

    [SerializeField]
    private CinemachineBrain m_Brain;

    [SerializeField]
    private CinemachineCamera m_PodiumCamera;

    [SerializeField]
    private CinemachineCamera m_FinishedCamera;    
    
    [SerializeField]
    private CinemachineCamera m_CountdownCamera;

    [SerializeField]
    private CinemachineCamera m_DollyCamera;

    [SerializeField]
    private CinemachineTargetGroup m_TargetGroup;
    
    private GameObject m_Podium;
    private GameObject m_Leader;

    public CinemachineCamera DollyCamera
    {
        get => m_DollyCamera;
    }

    public CinemachineCamera PodiumCamera
    {
        get => m_PodiumCamera;
    }

    public CinemachineCamera CountdownCamera
    {
        get => m_CountdownCamera;
    }

    public CinemachineCamera FinishedCamera
    {
        get => m_FinishedCamera;
    }

    protected CinemachineTargetGroup TargetGroup
    {
        get => m_TargetGroup;
        private set => m_TargetGroup = value;
    }

    protected GameObject Podium
    {
        get => m_Podium;
        private set => m_Podium = value;
    }

    protected GameObject Leader
    {
        get => m_Leader;
        private set => m_Leader = value;
    }

    private void Awake()
    {
        AddListeners();

        m_Brain.enabled = false;

        DollyCamera.Priority = 0;
        PodiumCamera.Priority = 0;
        FinishedCamera.Priority = 0;
        CountdownCamera.Priority = 0;
    }

    public void SetTarget(GameObject target)
    {
        TargetChanged?.Invoke(target);
    }

    public void SetTargetGroup(CinemachineTargetGroup targetGroup)
    {
        TargetGroup = targetGroup;
        TargetGroupChanged?.Invoke(TargetGroup);
    }

    public void AddToTargetGroup(GameObject newTarget, float weight, float radius)
    {
        if (TargetGroup == null)
        {
            return;
        }

        foreach (var targetGroupMember in TargetGroup.Targets)
        {
            if (targetGroupMember.Object == newTarget.transform)
            {
                return;
            }
        }

        TargetGroup.AddMember(newTarget.transform, weight, radius);
        TargetGroupChanged?.Invoke(TargetGroup);
    }

    public void InitializeAndStartRaceCamera()
    {
        if (TargetGroup == null || TargetGroup.Targets.Count == 0)
        {
            return;
        }

        m_Brain.enabled = true;
    }

    public void RemoveFromTargetGroup(GameObject oldTarget)
    {
        if (IsInTargetGroup(oldTarget.transform))
        {
            TargetGroup.RemoveMember(oldTarget.transform);
            TargetGroupChanged?.Invoke(TargetGroup);
        }
    }

    public void ClearTargetGroup()
    {
        if (TargetGroup == null)
        {
            return;
        }

        for (var i = TargetGroup.Targets.Count - 1; i >= 0; i--)
        {
            TargetGroup.RemoveMember(TargetGroup.Targets[i].Object);
        }

        TargetGroupChanged?.Invoke(TargetGroup);
    }

    private bool IsInTargetGroup(Transform newTarget)
    {
        foreach (var target in TargetGroup.Targets)
        {
            if (target.Object == newTarget)
            {
                return true;
            }
        }

        return false;
    }

    private void AddListeners()
    {
        RaceManager.Current.OnRaceStatus += OnRaceStatusChanged;
    }

    private void OnRaceStatusChanged(RaceStatus raceStatus)
    {
        switch (raceStatus)
        {
            case RaceStatus.Countdown:
                SetBlend(Styles.HardIn, 1);
                DollyCamera.Priority = 1;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 0;
                CountdownCamera.Priority = 0;
                break;
            case RaceStatus.Race:
                SetBlend(Styles.EaseOut, 3);
                DollyCamera.Priority = 1;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 0;
                CountdownCamera.Priority = 0;
                break;
            case RaceStatus.HeatEnd:
                SetBlend(Styles.HardIn, 1);
                DollyCamera.Priority = 0;
                PodiumCamera.Priority = 1;
                FinishedCamera.Priority = 0;
                CountdownCamera.Priority = 0;
                break;
            case RaceStatus.Finished:
                SetBlend(Styles.HardIn, 1);
                DollyCamera.Priority = 0;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 1;
                CountdownCamera.Priority = 0;
                break;
            default:
                break;
        }
    }

    private void SetBlend(Styles style, float time)
    {
        m_Brain.DefaultBlend = new CinemachineBlendDefinition (style, time);
    }

    private void RemoveListeners()
    {
        RaceManager.Current.OnRaceStatus -= OnRaceStatusChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
