using Cinemachine;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private CinemachineVirtualCamera m_RaceCamera;
    private CinemachineTargetGroup m_TargetGroup;

    private void Awake()
    {
        if (gameObject.TryGetComponent(out CameraControl cameraControl))
        {
            m_RaceCamera = cameraControl.RaceCamera;
        }
    }

    public void SetTargetGroup()
    {
        if (!Instantiate(GameManager.Current.TargetGroup).TryGetComponent(out CinemachineTargetGroup targetGroup))
        {
            return;
        }

        m_TargetGroup = targetGroup;

        m_RaceCamera.Follow = m_TargetGroup.transform;
        m_RaceCamera.LookAt = m_TargetGroup.transform;
    }

    public void AddToTargetGroup(GameObject newTarget, float weight, float radius)
    {
        if (m_TargetGroup == null)
        {
            return;
        }

        foreach (var targetGroupMember in m_TargetGroup.m_Targets)
        {
            if (targetGroupMember.target == newTarget.transform)
            {
                return;
            }
        }

        m_TargetGroup.AddMember(newTarget.transform, weight, radius);
    }

    public void RemoveFromTargetGroup(GameObject oldTarget)
    {
        if (IsInTargetGroup(oldTarget.transform))
        {
            m_TargetGroup.RemoveMember(oldTarget.transform);
        }
    }

    public void ClearTargetGroup()
    {
        if (m_TargetGroup == null)
        {
            return;
        }

        for (var i = m_TargetGroup.m_Targets.Length - 1; i >= 0; i--)
        {
            m_TargetGroup.RemoveMember(m_TargetGroup.m_Targets[i].target);
        }
    }

    private bool IsInTargetGroup(Transform newTarget)
    {
        foreach (var target in m_TargetGroup.m_Targets)
        {
            if (target.target == newTarget)
            {
                return true;
            }
        }

        return false;
    }
}
