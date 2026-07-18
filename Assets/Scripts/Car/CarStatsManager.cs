using System;
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

    public void AddTimedModifier(Stat stat, ModifierType modifierType, float modifierValue, float duration)
    {
        m_ActiveModifiers.Add(new TimedModifier(stat, modifierType, modifierValue, duration));
    }

    public void AddConditionalModifier(Stat stat, ModifierType modifierType, Func<float> modifierValue, Func<bool> modifierCondition)
    {
        m_ActiveModifiers.Add(new ConditionalModifier(stat, modifierType, modifierValue, modifierCondition));
    }

    public float GetModifierAmount(Stat carStat)
    {
        var modifierAmount = 1f;
        var modifiers = m_ActiveModifiers.Where(x => x.Stat == carStat);

        foreach (var modifier in modifiers)
        {
            var modifierValue = modifier.GetValue();

            switch (modifier.ModifierType)
            {
                case ModifierType.Addative:
                    modifierAmount += modifierValue;
                    break;
                case ModifierType.Multiplier:
                    modifierAmount *= modifierValue;
                    break;
            }

        }

        return modifierAmount;
    }
}
