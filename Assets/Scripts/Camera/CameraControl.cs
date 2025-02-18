using Cinemachine;
using UnityEngine;
using static Cinemachine.CinemachineBlendDefinition;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private CinemachineBrain m_Brain;

    [SerializeField]
    private CinemachineVirtualCamera m_RaceCamera;

    [SerializeField]
    private CinemachineVirtualCamera m_PodiumCamera;

    [SerializeField]
    private CinemachineVirtualCamera m_FinishedCamera;    
    
    [SerializeField]
    private CinemachineVirtualCamera m_CountdownCamera;

    private CinemachineTargetGroup m_TargetGroup;

    public CinemachineVirtualCamera RaceCamera
    {
        get => m_RaceCamera;
    }

    public CinemachineVirtualCamera PodiumCamera
    {
        get => m_PodiumCamera;
    }

    public CinemachineVirtualCamera CountdownCamera
    {
        get => m_CountdownCamera;
    }

    public CinemachineVirtualCamera FinishedCamera
    {
        get => m_FinishedCamera;
    }

    private void Awake()
    {
        AddListeners();
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
                SetBlend(Style.HardIn, 1);

                RaceCamera.Priority = 0;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 0;
                CountdownCamera.Priority = 1;
                break;
            case RaceStatus.Race:
                SetBlend(Style.EaseOut, 3);

                RaceCamera.Priority = 1;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 0;
                CountdownCamera.Priority = 0;
                break;
            case RaceStatus.HeatEnd:
                SetBlend(Style.HardIn, 1);

                RaceCamera.Priority = 0;
                PodiumCamera.Priority = 1;
                FinishedCamera.Priority = 0;
                CountdownCamera.Priority = 0;
                break;
            case RaceStatus.Finished:
                SetBlend(Style.HardIn, 1);

                RaceCamera.Priority = 0;
                PodiumCamera.Priority = 0;
                FinishedCamera.Priority = 1;
                CountdownCamera.Priority = 0;
                break;
            default:
                break;
        }
    }

    private void SetBlend(Style style, float time)
    {
        m_Brain.m_DefaultBlend = new CinemachineBlendDefinition(style, time);
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
