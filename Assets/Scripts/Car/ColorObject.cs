using UnityEngine;

[CreateAssetMenu(fileName = "ColorObject", menuName = "Scriptable Objects/ColorObject")]
public class ColorObject : ScriptableObject
{
    [SerializeField]
    private PlayerColor m_PlayerColor;

    [SerializeField]
    private Material m_Material;

    public Material Material
    {
        get => m_Material;
    }

    public PlayerColor PlayerColor
    {
        get => m_PlayerColor;
    }
}
