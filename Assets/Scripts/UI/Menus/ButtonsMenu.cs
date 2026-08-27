using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonsMenu : Menu
{
    [SerializeField]
    private Selectable startSelection;

    private Selectable m_LastSelected;

    public Selectable StartSelection
    {
        get => startSelection;
    }

    protected Selectable LastSelected
    {
        get => m_LastSelected;
        set => m_LastSelected = value;
    }

    private void Update()
    {
        if (EventSystem.currentSelectedGameObject == null)
        {
            if (LastSelected == null)
            {
                EventSystem.SetSelectedGameObject(startSelection.gameObject);
                return;
            }

            EventSystem.SetSelectedGameObject(LastSelected.gameObject);
        }
    }
}
