using UnityEngine;

public class NpcCar : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 18f;

    [SerializeField]
    private Transform m_Target;

    private void Update()
    {
        transform.Translate(transform.forward.normalized * m_Speed * Time.deltaTime, Space.World);

        transform.position = Vector3.MoveTowards(
            transform.position,
            m_Target.position,
            m_Speed * Time.deltaTime
        );

        transform.LookAt(m_Target.position);
    }
}
