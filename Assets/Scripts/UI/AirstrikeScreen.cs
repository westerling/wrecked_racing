using System.Collections.Generic;
using UnityEngine;

public class AirstrikeScreen : MonoBehaviour
{
    [SerializeField]
    private Airstrike[] m_Airstrikes;

    public void SetupAirstrike(List<PlayerCar> cars)
    {
        for (var i = 0; i < m_Airstrikes.Length; i++)
        {
            if (cars.Count > i)
            {
                m_Airstrikes[i].AddCar(cars[i]);
                m_Airstrikes[i].gameObject.SetActive(true);
            }
            else
            {
                m_Airstrikes[i].gameObject.SetActive(false);
            }
        }
    }
}
