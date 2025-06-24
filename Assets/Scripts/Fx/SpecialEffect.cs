using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    [SerializeField]
    private ParticleType m_ParticleType;

    public ParticleType ParticleType
    {
        get => m_ParticleType;
    }
}
