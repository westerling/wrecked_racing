using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    [SerializeField]
    private UnityEvent m_ButtonSelectedEvent;

    public UnityEvent ButtonSelectedEvent
    {
        get => m_ButtonSelectedEvent;
    }

    public void ButtonSelected()
    {
        ButtonSelectedEvent?.Invoke();
    }
}
