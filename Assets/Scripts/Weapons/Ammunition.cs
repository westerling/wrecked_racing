using UnityEngine;

public abstract class Ammunition : MonoBehaviour
{
    private WeaponType m_WeaponType;

    public WeaponType WeaponType
    {
        get => m_WeaponType;
        protected set => m_WeaponType = value;
    }

    protected void Deactivate()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(AmmunitionPool.Current.transform);
    }
}
