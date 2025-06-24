using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public event Action<float> Accelerate;
    public event Action<float> Brake;
    public event Action<float> Steer;
    public event Action FireStarted;
    public event Action FireStopped;

    public event Action<Player, MenuNavigation> NavigateMenu;
    public event Action<Player> BackMenu;
    public event Action<Player> GoMenu;

    private Player m_Player;
    private InputActionMap m_RaceInputActionMap;
    private InputActionMap m_MenuInputActionMap;

    private const string RaceInputActionMap = "Race";
    private const string MenuInputActionMap = "Menu";

    [SerializeField]
    private PlayerInput m_Controls;

    private void Awake()
    {
        if (TryGetComponent(out Player player))
        {
            m_Player = player;
        }

        m_RaceInputActionMap = m_Controls.actions.FindActionMap(RaceInputActionMap);
        m_MenuInputActionMap = m_Controls.actions.FindActionMap(MenuInputActionMap);
    }

    private void Start()
    {
        GameManager.Current.GameStateChanged += OnGameStateChanged;
        AddRaceListeners();
        AddMenuListeners();

        SetActionMapStatus(GameState.Menu);
    }

    private void OnGameStateChanged(GameState gameState)
    {
        SetActionMapStatus(gameState);
    }

    private void SetActionMapStatus(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Menu:
                m_RaceInputActionMap.Disable();
                m_MenuInputActionMap.Enable();
                break;
            case GameState.Race:
                m_RaceInputActionMap.Enable();
                m_MenuInputActionMap.Disable();
                break;
            case GameState.Loading:
                m_RaceInputActionMap.Disable();
                m_MenuInputActionMap.Disable();
                break;
            default:
                m_RaceInputActionMap.Disable();
                m_MenuInputActionMap.Enable();
                break;
        }
    }

    private void AddMenuListeners()
    {
        m_Controls.actions["NavigateUp"].performed += NavigateUpPerformed;
        m_Controls.actions["NavigateDown"].performed += NavigateDownPerformed;
        m_Controls.actions["NavigateLeft"].performed += NavigateLeftPerformed;
        m_Controls.actions["NavigateRight"].performed += NavigateRightPerformed;
        m_Controls.actions["GoBackMenu"].performed += NavigateBackPerformed;
        m_Controls.actions["EnterMenu"].performed += NavigateForwardPerformed;
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
        m_Controls.actions["Fire"].canceled += FireStoppedPerformed;
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
        FireStarted?.Invoke();
    }

    private void FireStoppedPerformed(InputAction.CallbackContext obj)
    {
        FireStopped?.Invoke();
    }

    private void NavigateRightPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(m_Player, MenuNavigation.Right);
    }

    private void NavigateLeftPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(m_Player, MenuNavigation.Left);
    }

    private void NavigateDownPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(m_Player, MenuNavigation.Down);
    }

    private void NavigateUpPerformed(InputAction.CallbackContext obj)
    {
        NavigateMenu?.Invoke(m_Player, MenuNavigation.Up);
    }

    private void NavigateForwardPerformed(InputAction.CallbackContext obj)
    {
        GoMenu?.Invoke(m_Player);
    }

    private void NavigateBackPerformed(InputAction.CallbackContext obj)
    {
        BackMenu?.Invoke(m_Player);
    }

    private void RemoveRaceListeners()
    {
        m_Controls.actions["Accelerate"].performed -= AcceleratePerfomed;
        m_Controls.actions["Accelerate"].canceled -= AcceleratePerfomed;
        m_Controls.actions["Brake"].performed -= BrakePerformed;
        m_Controls.actions["Brake"].canceled -= BrakePerformed;
        m_Controls.actions["Steer"].performed -= SteerPerformed;
        m_Controls.actions["Steer"].canceled -= SteerPerformed;
        m_Controls.actions["Fire"].performed -= FirePerfomed;
        m_Controls.actions["Fire"].canceled -= FireStoppedPerformed;
    }

    private void RemoveMenuListeners()
    {
        m_Controls.actions["NavigateUp"].performed -= NavigateUpPerformed;
        m_Controls.actions["NavigateDown"].performed -= NavigateDownPerformed;
        m_Controls.actions["NavigateLeft"].performed -= NavigateLeftPerformed;
        m_Controls.actions["NavigateRight"].performed -= NavigateRightPerformed;
        m_Controls.actions["GoBackMenu"].performed -= NavigateBackPerformed;
        m_Controls.actions["EnterMenu"].performed -= NavigateForwardPerformed;
    }

    private void OnDestroy()
    {
        GameManager.Current.GameStateChanged -= OnGameStateChanged;
        RemoveRaceListeners();
        RemoveMenuListeners();
    }
}
