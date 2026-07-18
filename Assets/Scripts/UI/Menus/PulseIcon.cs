using UnityEngine;

public class PulseIcon : MonoBehaviour
{
    [SerializeField]
    private float m_PulseSpeed = 2f;
    
    [SerializeField]
    private float m_PulseAmount = 0.2f;

    private Vector3 originalScale;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * m_PulseSpeed) * m_PulseAmount;
        transform.localScale = originalScale * scale;
    }
}
