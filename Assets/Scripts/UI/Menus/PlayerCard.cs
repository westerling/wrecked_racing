using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField]
    private Image m_Image;

    [SerializeField]
    private InputImageSelector m_InputImageSelector;

    private Player m_Player;
    private PlayerColor m_Color;
    private bool m_Ready;

    public Player Player
    {
        get => m_Player;
        set => m_Player = value;
    }

    public bool Ready
    {
        get => m_Ready;
        set => m_Ready = value;
    }

    public PlayerColor Color
    {
        get => m_Color;
        set => m_Color = value;
    }

    public Image Image
    {
        get => m_Image;
        set => m_Image = value;
    }

    public InputImageSelector InputImageSelector
    {
        get => m_InputImageSelector;
    }
}
