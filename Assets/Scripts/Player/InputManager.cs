using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<float> Accelerate;
    public event Action<float> Brake;
    public event Action<float> Steer;
    public event Action Fire;

    public event Action<MenuNavigation> NavigateMenu;
    public event Action BackMenu;
    public event Action GoMenu;

    [SerializeField]
    private PlayerInput m_Controls;

    private Player m_Player;

    private void Awake()
    {
        if (TryGetComponent(out Player player))
        {
            m_Player = player;
        }
    }

    private void Start()
    {
        AddRaceListeners();
        AddMenuListeners();
        
    }

    private void AddMenuListeners()
    {
        m_Controls.actions["NavigateUp"].performed += NavigateUpPerformed;
        m_Controls.actions["NavigateDown"].performed += NavigateDownPerformed;
        m_Controls.actions["NavigateLeft"].performed += NavigateLeftPerformed;
        m_Controls.actions["NavigateLeft"].performed += NavigateLeftPerformed;
        m_Controls.actions["BackMenu"].performed += NavigateBackPerformed;
        m_Controls.actions["GoMenu"].performed += NavigateForwardPerformed;
    }

    private void AddRaceListeners()
    {
        m_Controls.actions["Accelerate"].performed += AcceleratePerfomed;
        m_Controls.actions["Accelerate"].canceled += AcceleratePerfomed;
        m_Controls.actions["Brake"].performed += BrakePerformed;
        m_Controls.actions["Brake"].canceled += BrakePerformed;
        m_Controls.actions["Steer"].performed += SteerPerformed;
        m_Controls.actions["Steer"].canceled += SteerPerformed;
        m_Controls.actions["Fire"].performed += FirePerfomed;
        m_Controls.actions["Fire"].canceled += FirePerfomed;
    }

    public void SetActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
        {
            return;
        }


    }

    private void SteerPerformed(InputAction.CallbackContext obj)
    {
        Steer?.Invoke(obj.ReadValue<float>());
    }

    private void BrakePerformed(InputAction.CallbackContext obj)
    {
        Brake?.Invoke(obj.ReadValue<float>());
    }

    private void AcceleratePerfomed(InputAction.CallbackContext obj)
    {
        Accelerate?.Invoke(obj.ReadValue<float>());
    }

    private void FirePerfomed(InputAction.CallbackContext obj)
    {
        Fire?.Invoke();
    }

    private void NavigateRightPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(MenuNavigation.Right);
    }

    private void NavigateLeftPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(MenuNavigation.Left);
    }

    private void NavigateDownPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(MenuNavigation.Down);
    }

    private void NavigateUpPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(MenuNavigation.Up);
    }

    private void NavigateForwardPerformed(InputAction.CallbackContext obj)
    {
        BackMenu?.Invoke();
    }

    private void NavigateBackPerformed(InputAction.CallbackContext obj)
    {
        GoMenu?.Invoke();
    }

    private void OnDestroy()
    {
        m_Controls.actions["Accelerate"].performed -= AcceleratePerfomed;
        m_Controls.actions["Accelerate"].canceled -= AcceleratePerfomed;
        m_Controls.actions["Brake"].performed -= BrakePerformed;
        m_Controls.actions["Brake"].canceled -= BrakePerformed;
        m_Controls.actions["Steer"].performed -= SteerPerformed;
        m_Controls.actions["Steer"].canceled -= SteerPerformed;
        m_Controls.actions["Fire"].performed -= FirePerfomed;
        m_Controls.actions["Fire"].canceled -= FirePerfomed;

        m_Controls.actions["NavigateUp"].performed -= NavigateUpPerformed;
        m_Controls.actions["NavigateDown"].performed -= NavigateDownPerformed;
        m_Controls.actions["NavigateLeft"].performed -= NavigateLeftPerformed;
        m_Controls.actions["NavigateLeft"].performed -= NavigateLeftPerformed;
        m_Controls.actions["BackMenu"].performed -= NavigateBackPerformed;
        m_Controls.actions["GoMenu"].performed -= NavigateForwardPerformed;

    }
}
