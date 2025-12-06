using Unity.Cinemachine;
using System;
using UnityEngine;
using static Unity.Cinemachine.CinemachineBlendDefinition;

public class CameraControl : MonoBehaviour
{
    public event Action<CinemachineTargetGroup> TargetGroupChanged;

    [SerializeField]
    private CinemachineBrain m_Brain;

    [SerializeField]
    private CinemachineCamera m_PodiumCamera;

    [SerializeField]
    private CinemachineCamera m_FinishedCamera;    
    
    [SerializeField]
    private CinemachineCamera m_DollyCamera;

    [SerializeField]
    private GameObject m_AudioListener;

    private CinemachineTargetGroup m_TargetGroup;

    public CinemachineCamera DollyCamera
    {
        get => m_DollyCamera;
    }

    public CinemachineCamera PodiumCamera
    {
        get => m_PodiumCamera;
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

    private void Awake()
    {
        AddListeners();
    }

    private void LateUpdate()
    {
        SetAudioListener();
    }

    private void SetAudioListener()
    {
        var groupCenter = TargetGroup.transform.position;
        var cameraEuler = DollyCamera.transform.eulerAngles;

        m_AudioListener.transform.SetPositionAndRotation(
            groupCenter + Vector3.up * 1f,
            Quaternion.Euler(0, cameraEuler.y, 0));
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
        RaceManager.Current.RaceStatusChanged += OnRaceStatusChanged;
    }

    private void OnRaceStatusChanged(RaceStatus raceStatus)
    {
        switch (raceStatus)
        {
            case RaceStatus.Countdown:
            case RaceStatus.Race:
                SetBlend(Styles.Cut, 0.5f);
                DollyCamera.Priority = 1;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 0;
                break;
            case RaceStatus.HeatEnd:
                SetBlend(Styles.Cut, 0.5f);
                DollyCamera.Priority = 0;
                PodiumCamera.Priority = 1;
                FinishedCamera.Priority = 0;
                break;
            case RaceStatus.Finished:
                SetBlend(Styles.Cut, 0.5f);
                DollyCamera.Priority = 0;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 1;
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
        RaceManager.Current.RaceStatusChanged -= OnRaceStatusChanged;
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
