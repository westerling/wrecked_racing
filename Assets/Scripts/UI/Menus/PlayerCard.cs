using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField]
    private Image m_ColorImage;

    [SerializeField]
    private Image m_Icon;

    [SerializeField]
    private GameObject[] m_KeyboardImages;

    [SerializeField]
    private GameObject[] m_GamepadImages;

    [SerializeField]
    private GameObject m_ColorCarousel;

    [SerializeField]
    private TMP_Text m_ReadyText;

    [SerializeField]
    private TMP_Text m_StatusText;

    [SerializeField]
    private TMP_Text m_ColorText;

    [SerializeField]
    private Color m_DefaultColor;

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

    public Image Icon
    {
        get => m_Icon;
        set => m_Icon = value;
    }

    public TMP_Text StatusText
    {
        get => m_StatusText;
    }
        
    public TMP_Text ColorText
    {
        get => m_ColorText;
    }

    public TMP_Text ReadyText
    {
        get => m_ReadyText;
    }
    
    public GameObject[] KeyboardImages
    {
        get => m_KeyboardImages;
    }

    public GameObject[] GamepadImages
    {
        get => m_GamepadImages;
    }

    public Image ColorImage
    {
        get => m_ColorImage;
    }

    public GameObject ColorCarousel
    {
        get => m_ColorCarousel;
    }
}
