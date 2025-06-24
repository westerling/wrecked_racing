using System;

public class ConditionalModifier : StatModifier
{
    private Func<float> m_GetValue;
    private Func<bool> m_Condition;

    public ConditionalModifier(Stat stat, Func<float> getValue, Func<bool> condition) : base(stat)
    {
        m_Condition = condition;
        GetValue = getValue;
    }

    public Func<float> GetValue
    {
        get => m_GetValue;
        set => m_GetValue = value;
    }

    public override bool IsExpired()
    {
        return !m_Condition();
    }
}
