using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCardSprite", menuName = "Scriptable Objects/PlayerCardSprite")]
public class PlayerCardSprite : ScriptableObject
{
    [SerializeField]
    private Sprite m_PlayerReady;

    [SerializeField]
    private Sprite m_PlayerNotReady;

    [SerializeField]
    private Sprite m_Locked;

    [SerializeField]
    private Sprite m_Open;

    [SerializeField]
    private PlayerColor m_PlayerColor;

    public PlayerColor PlayerColor
    {
        get => m_PlayerColor;
    }
    
    public Sprite PlayerNotReady
    {
        get => m_PlayerNotReady;
    }
    
    public Sprite PlayerReady
    {
        get => m_PlayerReady;
    }

    public Sprite Locked
    {
        get => m_Locked;
    }
    
    public Sprite Open
    {
        get => m_Open;
    }
}
