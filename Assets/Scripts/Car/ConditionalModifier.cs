using System;

public class ConditionalModifier : StatModifier
{
    private Func<float> m_Value;
    private Func<bool> m_Condition;

    public ConditionalModifier(Stat stat, ModifierType modifierType, Func<float> value, Func<bool> condition) : base(stat, modifierType)
    {
        m_Condition = condition;
        Value = value;
    }

    public Func<float> Value
    {
        get => m_Value;
        set => m_Value = value;
    }

    public override float GetValue()
    {
        return Value();
    }

    public override bool IsExpired()
    {
        return !m_Condition();
    }
}
