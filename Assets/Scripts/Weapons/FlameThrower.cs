using UnityEngine;

public class FlameThrower : Weapon
{
    [Header("Sounds")]
    [SerializeField]
    private Sound m_SoundEffect;

    protected override void Fire()
    {
        SoundFxManager.Current.PlaySoundClip(m_SoundEffect, transform);

        var car = GetComponentInParent<Car>();

        if (car == null)
        {
            return;
        }

        car.StatusManager.AddTimedModifier(Stat.Speed, 1.1f, 0.1f);
    }
}
