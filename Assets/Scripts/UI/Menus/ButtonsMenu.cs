using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonsMenu : Menu
{
    [SerializeField]
    private Selectable startSelection;

    public Selectable StartSelection
    {
        get => startSelection;
    }
}
