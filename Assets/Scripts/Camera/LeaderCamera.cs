using UnityEngine;

public class LeaderCamera : MonoBehaviour
{
    private Transform m_Target;
    private Vector3 m_Offset = new Vector3 (0f, 5f, -10f);
    private float m_SmoothSpeed = 5f;


    void Start()
    {
        AddListeners();
        AddStarterCar();
    }

    void LateUpdate()
    {
        if (m_Target == null)
        {
            return;
        }

        Vector3 desiredPosition = m_Target.position + m_Offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, m_SmoothSpeed * Time.deltaTime);

        transform.LookAt(m_Target);
    }

    private void AddStarterCar()
    {
        if (RaceManager.Current.Cars.Count > 0)
        {
            m_Target = RaceManager.Current.Cars[0].transform;
        }
    }

    private void AddListeners()
    {
        RaceManager.Current.LeaderChanged += OnLeaderChange;
    }

    private void OnLeaderChange(GameObject leader)
    {
        m_Target = leader.transform;
    }

    private void RemoveListeners()
    {
        RaceManager.Current.LeaderChanged -= OnLeaderChange;
    }   
    
    private void OnDestroy()
    {
        RemoveListeners();
    }
}
