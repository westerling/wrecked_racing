using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    private UnityEvent m_ButtonSelectedEvent;

    public event Action<Selectable> ButtonHighlighted;

    public UnityEvent ButtonSelectedEvent
    {
        get => m_ButtonSelectedEvent;
    }

    public void ButtonSelected()
    {
        ButtonSelectedEvent?.Invoke();
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (TryGetComponent(out Selectable selectable))
        {
            ButtonHighlighted?.Invoke(selectable);
        }
    }
}
