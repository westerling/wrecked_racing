using UnityEngine;

public class NpcCar : Car
{
    [SerializeField]
    private bool m_ActiveOnStart;

    protected override void Awake()
    {
        base.Awake();

        CarStatus = CarStatus.Active; 

        IsAi = true;
    }
}
