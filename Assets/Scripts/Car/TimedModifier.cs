public class TimedModifier : StatModifier
{
    private float m_TimeRemaining;
    private float m_Value;

    public float Value
    {
        get => m_Value;
        private set => m_Value = value;
    }

    public TimedModifier(Stat stat, float value, float duration) : base(stat)
    {
        m_TimeRemaining = duration;
        Value = value;
    }

    public void SetTime(float time)
    {
        m_TimeRemaining -= time;
    }

    public override bool IsExpired()
    {
        return m_TimeRemaining <= 0;
    }
}
