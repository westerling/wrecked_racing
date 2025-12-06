using System;
using UnityEngine;

[Serializable]
public class TrackInfo
{
    [SerializeField]
    private string m_TrackName;

    [SerializeField]
    private string m_TrackDescription;

    [SerializeField]
    private Sprite m_TrackImage;

    [SerializeField]
    private int m_SceneIndex;

    public string TrackName 
    {
        get => m_TrackName; 
    }
    
    public string TrackDescription
    {
        get => m_TrackDescription; 
    }

    public Sprite TrackImage 
    {
        get => m_TrackImage; 
    }

    public int SceneIndex 
    {
        get => m_SceneIndex; 
    }
}
