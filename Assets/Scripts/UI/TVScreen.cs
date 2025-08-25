using UnityEngine;

public class TVScreen : MonoBehaviour
{
    [SerializeField]
    private Transform m_FocusPoint;

    public Transform FocusPoint
    {
        get => m_FocusPoint;
    }
}
