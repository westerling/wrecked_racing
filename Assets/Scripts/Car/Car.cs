using System;
using UnityEngine;

public class Car : MonoBehaviour
{
    public event Action<bool> CarStatusChanged;

    [SerializeField]
    private Rigidbody m_Rigidbody;

    [SerializeField]
    private Transform m_CenterOfMass;

    [SerializeField]
    private CarStatusManager m_StatusManager;

    [SerializeField]
    private Stats m_Stats;

    [SerializeField]
    private Wheel[] m_Wheels;

    private InputManager m_InputManager;

    public Stats Stats
    {
        get => m_Stats;
    }

    public Rigidbody Rigidbody
    {
        get => m_Rigidbody;
    }

    public Transform CenterOfMass
    {
        get => m_CenterOfMass;
    }

    public InputManager InputManager
    {
        get => m_InputManager;
        private set => m_InputManager = value;
    }

    public Wheel[] Wheels 
    {
        get => m_Wheels; 
    }

    public CarStatusManager StatusManager
    { 
        get => m_StatusManager;
    }

    private void OnEnable()
    {
        var player = GetComponentInParent<Player>();

        if (player != null)
        {
            if (player.TryGetComponent(out InputManager inputManager))
            {
                InputManager = inputManager;
            }
        }
    }

    private void Awake()
    {
        Rigidbody.centerOfMass = m_CenterOfMass.localPosition;
    }
}
