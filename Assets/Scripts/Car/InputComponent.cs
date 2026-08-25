public abstract class InputComponent : CarComponent
{
    protected abstract void AddListeners();
    protected abstract void RemoveListeners();

    protected virtual void Start()
    {
        AddListeners();
    }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }
}
