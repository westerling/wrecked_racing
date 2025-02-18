using UnityEngine.Events;
using UnityEngine;

public class SelectionButton : CustomButton
{
    [SerializeField]
    private UnityEvent m_OnLeft;

    [SerializeField]
    private UnityEvent m_OnRight;

    public void OnLeft()
    {
        m_OnLeft?.Invoke();
    }

    public void OnRight()
    {
        m_OnRight?.Invoke();
    }
}
