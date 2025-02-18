using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarStatsManager : CarComponent
{
    private List<StatModifier> m_ActiveModifiers = new List<StatModifier>();

    private void Update()
    {
        for (var i = m_ActiveModifiers.Count - 1; i >= 0; i--)
        {
            var modifier = m_ActiveModifiers[i];

            if (modifier is TimedModifier timedModifier)
            {
                timedModifier.SetTime(Time.deltaTime);
            }

            if (modifier.IsExpired())
            {
                m_ActiveModifiers.RemoveAt(i);
            }
        }
    }

    public void AddTimedModifier(Stat stat, float modifierValue, float duration)
    {
        m_ActiveModifiers.Add(new TimedModifier(stat, modifierValue, duration));
    }

    public void AddConditionalModifier(Stat stat, float modifierValue, System.Func<bool> condition)
    {
        m_ActiveModifiers.Add(new ConditionalModifier(stat, modifierValue, condition));
    }

    public float GetModifierAmount(Stat carStat)
    {
        var modifierValue = 1f;

        if (m_ActiveModifiers.Count != 0)
        {
            var modifiers = m_ActiveModifiers.Where(x => x.Stat == carStat);

            foreach (var modifier in modifiers)
            {
                modifierValue *= modifier.Value;
            }
        }

        return modifierValue;
    }
}
