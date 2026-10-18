using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCardSprite", menuName = "Scriptable Objects/PlayerCardSprite")]
public class PlayerCardSprite : ScriptableObject
{
    [SerializeField]
    private Sprite m_BackgroundImage;

    [SerializeField]
    private Sprite m_Icon;

    [SerializeField]
    private PlayerColor m_PlayerColor;

    [SerializeField]
    private Color m_Color;

    [SerializeField]
    private Color m_ColorUnavailable;

    public PlayerColor PlayerColor
    {
        get => m_PlayerColor;
    }
    
    public Color Color
    {
        get => m_Color;
    }
    
    public Sprite BackgroundImage
    {
        get => m_BackgroundImage;
    }

    public Sprite Icon
    {
        get => m_Icon;
    }

    public Color ColorUnavailable
    {
        get => m_ColorUnavailable;
    }
}
