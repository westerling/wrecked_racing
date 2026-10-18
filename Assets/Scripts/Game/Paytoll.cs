using System.Collections;
using UnityEngine;

public class Paytoll : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Gate;

    [SerializeField]
    private bool m_Delay = false;


    private float m_Interval = 4f;
    private float m_RotationDuration = 1f;
    private float m_OpenDuration = 4f;

    private Quaternion m_ClosedRotation;
    private Quaternion m_OpenRotation;

    private void Start()
    {
        m_ClosedRotation = m_Gate.transform.localRotation;
        m_OpenRotation = m_ClosedRotation * Quaternion.Euler(0f, 0f, 90);

        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        var delay = m_Delay ? 4 : 0f;

        yield return new WaitForSeconds(delay);

        StartCoroutine(GateRoutine());
    }

    private IEnumerator GateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_Interval);

            yield return RotateGate(m_OpenRotation);

            yield return new WaitForSeconds(m_OpenDuration);

            yield return RotateGate(m_ClosedRotation);
        }
    }

    private IEnumerator RotateGate(Quaternion targetRotation)
    {
        Quaternion startRotation = m_Gate.transform.localRotation;
        float elapsed = 0f;

        while (elapsed < m_RotationDuration)
        {
            elapsed += Time.deltaTime;

            m_Gate.transform.localRotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                elapsed / m_RotationDuration
            );

            yield return null;
        }

        m_Gate.transform.localRotation = targetRotation;
    }
}
