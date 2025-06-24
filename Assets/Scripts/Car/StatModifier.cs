public abstract class StatModifier
{
    private Stat m_Stat;

    public Stat Stat
    {
        get => m_Stat;
        protected set => m_Stat = value;
    }

    public StatModifier(Stat stat)
    {
        Stat = stat;
    }

    public abstract bool IsExpired();
}
