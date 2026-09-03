using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonsMenu : Menu
{
    [SerializeField]
    private Selectable m_StartSelection;

    [SerializeField]
    private CustomButton[] m_Buttons;

    private Selectable m_LastSelected;

    public Selectable StartSelection
    {
        get => m_StartSelection;
    }

    protected Selectable LastSelected
    {
        get => m_LastSelected;
        set => m_LastSelected = value;
    }

    private void Awake()
    {
        AddListeners();
    }

    private void OnButtonHighlighted(Selectable selectable)
    {
        m_LastSelected = selectable;
    }

    private void Update()
    {
        if (EventSystem.currentSelectedGameObject == null)
        {
            if (LastSelected == null)
            {
                EventSystem.SetSelectedGameObject(m_StartSelection.gameObject);
                return;
            }

            EventSystem.SetSelectedGameObject(LastSelected.gameObject);
        }
    }

    private void AddListeners()
    {
        foreach (var button in m_Buttons)
        {
            button.ButtonHighlighted += OnButtonHighlighted;
        }
    }

    private void RemoveListeners()
    {
        foreach (var button in m_Buttons)
        {
            button.ButtonHighlighted -= OnButtonHighlighted;
        }
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
