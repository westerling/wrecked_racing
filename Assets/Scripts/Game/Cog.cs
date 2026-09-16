using UnityEngine;

public class Cog : MonoBehaviour
{
    private bool m_Rotate;
    private bool m_RotateClockwise;

    public bool Rotate
    {
        get => m_Rotate;
        set => m_Rotate = value;
    }
    public bool RotateClockwise
    {
        get => m_RotateClockwise;
        set => m_RotateClockwise = value;
    }

    void Update()
    {
        if (m_Rotate)
        {
            if (m_RotateClockwise)
            {
                transform.Rotate(25 * Time.deltaTime, 0f, 0f, Space.Self);
            }
            else
            {
                transform.Rotate(-25 * Time.deltaTime, 0f, 0f, Space.Self);
            }
        }
    }
}
