using UnityEngine;

public abstract class InputComponent : CarComponent
{
    protected abstract void AddListeners();
    protected abstract void RemoveListeners();

    private void Start()
    {
        AddListeners();
    }

    private void OnDestroy()
    {
        RemoveListeners();
    }
}
