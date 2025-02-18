using Cinemachine;
using System;
using UnityEngine;

public class LeaderCamera : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera m_FollowCamera;

    void Start()
    {
        AddListeners();
    }

    private void AddListeners()
    {
        RaceManager.Current.LeaderChange += OnLeaderChange;
    }

    private void OnLeaderChange(GameObject leader)
    {
        m_FollowCamera.Follow = leader.transform;
        m_FollowCamera.LookAt = leader.transform;
    }

    private void RemoveListeners()
    {
        RaceManager.Current.LeaderChange -= OnLeaderChange;
    }   
    
    private void OnDestroy()
    {
        RemoveListeners();
    }
}
