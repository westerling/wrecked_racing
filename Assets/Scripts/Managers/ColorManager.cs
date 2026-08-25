using System.Linq;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Current;

    [SerializeField]
    private ColorObject[] m_ColorObjects;

    private void Awake()
    {
        Current = this;
    }

    public Material GetMaterial(PlayerColor color)
    {
        var colorObject = m_ColorObjects.FirstOrDefault(x => x.PlayerColor == color);

        if (colorObject != null)
        {
            return colorObject.Material;
        }

        return null;
    }
}
