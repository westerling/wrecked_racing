using UnityEngine;
using UnityEngine.UI;

public class PointMarker : MonoBehaviour
{
    [SerializeField]
    private Image m_PointImage;

    public void SetPointImageActive(bool isActive)
    {
        if (isActive)
        {
            m_PointImage.color = Color.black;

            return;
        }

        m_PointImage.color = new Color(0f,0f,0f,0f);
    }
}
