using UnityEngine;

public abstract class Ammunition : MonoBehaviour
{
    private AmmunitionType m_AmmunitionType;

    public AmmunitionType AmmunitionType
    {
        get => m_AmmunitionType;
        protected set => m_AmmunitionType = value;
    }

    protected void Deactivate()
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(AmmunitionPool.Current.transform);
    }
}
