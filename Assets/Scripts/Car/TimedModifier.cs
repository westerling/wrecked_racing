using Unity.VisualScripting;

public class TimedModifier : StatModifier
{
    private float m_TimeRemaining;

    public TimedModifier(Stat stat, float value, float duration) : base(stat, value)
    {
        m_TimeRemaining = duration;
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
