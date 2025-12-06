using UnityEngine;

public class Player : MonoBehaviour
{
    private string m_Name;
    private bool m_IsAi;

    private PlayerColor m_Color;
    private InputType m_InputType;

    public string Name
    {
        get => m_Name;
        set => m_Name = value;
    }

    public bool IsAi
    {
        get => m_IsAi;
        set => m_IsAi = value;
    }

    public PlayerColor Color
    {
        get => m_Color;
        set => m_Color = value;
    }

    public InputType InputType
    {
        get => m_InputType;
        set => m_InputType = value;
    }
}
