public abstract class StatModifier
{
    private Stat m_Stat;
    private ModifierType m_ModifierType;

    public abstract float GetValue();

    public Stat Stat
    {
        get => m_Stat;
        protected set => m_Stat = value;
    }

    public ModifierType ModifierType
    {
        get => m_ModifierType;
        set => m_ModifierType = value;
    }

    public StatModifier(Stat stat, ModifierType modifierType)
    {
        Stat = stat;
        ModifierType = modifierType;
    }

    public abstract bool IsExpired();
}
