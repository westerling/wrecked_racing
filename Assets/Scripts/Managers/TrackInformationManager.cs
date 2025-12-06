using UnityEngine;
using UnityEngine.Splines;

public class TrackInformationManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] m_SongList;

    [SerializeField]
    private Checkpoint[] m_Checkpoints;

    [SerializeField]
    private SplineContainer m_DollyTrack;

    public static TrackInformationManager Current;

    public Checkpoint[] Checkpoints
    {
        get => m_Checkpoints;
    }

    public AudioClip[] SongList
    {
        get => m_SongList;
    }

    public SplineContainer DollyTrack
    {
        get => m_DollyTrack;
    }

    private void Awake()
    {
        Current = this;
    }
}
