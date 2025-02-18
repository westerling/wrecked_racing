using UnityEngine;

public abstract class StatModifier
{
    private float m_Value;
    private Stat m_Stat;

    public float Value
    {
        get => m_Value;
        protected set => m_Value = value;
    }
    public Stat Stat
    {
        get => m_Stat;
        protected set => m_Stat = value;
    }

    public StatModifier(Stat stat, float value)
    {
        Value = value;
    }

    public abstract bool IsExpired();
}
