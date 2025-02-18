public class ConditionalModifier : StatModifier
{
    private System.Func<bool> m_Condition;

    public ConditionalModifier(Stat stat, float value, System.Func<bool> condition) : base(stat, value)
    {
        m_Condition = condition;
    }

    public override bool IsExpired()
    {
        return !m_Condition();
    }
}
