using Unity.Cinemachine;
using System;
using UnityEngine;
using static Unity.Cinemachine.CinemachineBlendDefinition;
using System.Collections;
using UnityEngine.UI;

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

    [SerializeField]
    private Image m_FadeImage; 

    private CinemachineTargetGroup m_TargetGroup;
    private CinemachineCamera[] m_Cameras;

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
        AddCameras();
    }

    private void AddCameras()
    {
        m_Cameras = new[]
        {
            DollyCamera,
            PodiumCamera,
            FinishedCamera
        };
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
                SetBlend(Styles.Cut, 0f);
                StartCoroutine(FadeCamera(DollyCamera, true));
                break;
            case RaceStatus.Race:
                SetBlend(Styles.HardIn, 0.5f);
                StartCoroutine(FadeCamera(DollyCamera, false));
                break;
            case RaceStatus.HeatEnd:
                SetBlend(Styles.HardIn, 0.5f);
                StartCoroutine(FadeCamera(PodiumCamera, false));
                break;
            case RaceStatus.Finished:
                SetBlend(Styles.HardIn, 0.5f);
                StartCoroutine(FadeCamera(FinishedCamera, false));
                break;
            default:
                break;
        }
    }

    private IEnumerator FadeCamera(CinemachineCamera cameraIn, bool fade)
    {
        if (fade)
        {
            yield return Fade(0f, 1f, 0.2f);
            yield return new WaitForSeconds(0.5f);
        }

        foreach (var camera in m_Cameras)
        {
            camera.Priority = camera == cameraIn ? 1 : 0;
        }

        yield return new WaitForSeconds(0.5f);

        if (fade)
        {
            yield return Fade(1f, 0f, 0.2f);
        }
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        var color = m_FadeImage.color;
        color.a = from;
        m_FadeImage.color = color;

        var elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            color.a = Mathf.Lerp(from, to, elapsed / duration);
            m_FadeImage.color = color;

            yield return null;
        }

        color.a = to;
        m_FadeImage.color = color;
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
