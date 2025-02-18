using UnityEngine;

public class StartCheckpoint : Checkpoint
{
    [SerializeField]
    private Transform[] m_StartPositions;

    public Transform[] StartPositions 
    {
        get => m_StartPositions; 
    }
}
