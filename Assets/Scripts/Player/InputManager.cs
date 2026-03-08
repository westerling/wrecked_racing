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
    private PlayerInput m_PlayerInput;

    protected virtual void Awake()
    {
        if (TryGetComponent(out Player player))
        {
            m_Player = player;
        }

        m_RaceInputActionMap = m_PlayerInput.actions.FindActionMap(RaceInputActionMap);
        m_MenuInputActionMap = m_PlayerInput.actions.FindActionMap(MenuInputActionMap);
    }

    private void Start()
    {
        GameManager.Current.GameStateChanged += OnGameStateChanged;

        if (m_PlayerInput != null)
        {
            AddRaceListeners();
            AddMenuListeners();
        }
        
        SetActionMapStatus(GameState.Menu);
    }

    protected void SendAccelerate(float value)
    {
        Accelerate?.Invoke(value);
    }

    protected void SendBrake(float value)
    {
        Brake?.Invoke(value);
    }

    protected void SendSteer(float value)
    {
        Steer?.Invoke(value);
    }

    protected void SendFireStarted()
    {
        FireStarted?.Invoke();
    }

    protected void SendFireStopped()
    {
        FireStopped?.Invoke();
    }

    protected void SendNavigateMenu(MenuNavigation navigation)
    {
        NavigateMenu?.Invoke(m_Player, navigation);
    }

    protected void SendBackMenu()
    {
        BackMenu?.Invoke(m_Player);
    }

    protected void SendGoMenu()
    {
        GoMenu?.Invoke(m_Player);
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
        m_PlayerInput.actions["NavigateUp"].performed += NavigateUpPerformed;
        m_PlayerInput.actions["NavigateDown"].performed += NavigateDownPerformed;
        m_PlayerInput.actions["NavigateLeft"].performed += NavigateLeftPerformed;
        m_PlayerInput.actions["NavigateRight"].performed += NavigateRightPerformed;
        m_PlayerInput.actions["GoBackMenu"].performed += NavigateBackPerformed;
        m_PlayerInput.actions["EnterMenu"].performed += NavigateForwardPerformed;
    }

    private void AddRaceListeners()
    {
        m_PlayerInput.actions["Accelerate"].performed += AcceleratePerfomed;
        m_PlayerInput.actions["Accelerate"].canceled += AcceleratePerfomed;
        m_PlayerInput.actions["Brake"].performed += BrakePerformed;
        m_PlayerInput.actions["Brake"].canceled += BrakePerformed;
        m_PlayerInput.actions["Steer"].performed += SteerPerformed;
        m_PlayerInput.actions["Steer"].canceled += SteerPerformed;
        m_PlayerInput.actions["Fire"].performed += FirePerfomed;
        m_PlayerInput.actions["Fire"].canceled += FireStoppedPerformed;
    }

    private void SteerPerformed(InputAction.CallbackContext obj)
    {
        SendSteer(obj.ReadValue<float>());
    }

    private void BrakePerformed(InputAction.CallbackContext obj)
    {
        SendBrake(obj.ReadValue<float>());
    }

    private void AcceleratePerfomed(InputAction.CallbackContext obj)
    {
        SendAccelerate(obj.ReadValue<float>());
    }

    private void FirePerfomed(InputAction.CallbackContext obj)
    {
        SendFireStarted();
    }

    private void FireStoppedPerformed(InputAction.CallbackContext obj)
    {
        SendFireStopped();
    }

    private void NavigateRightPerformed(InputAction.CallbackContext obj)
    {
        SendNavigateMenu(MenuNavigation.Right);
    }

    private void NavigateLeftPerformed(InputAction.CallbackContext obj)
    {
        SendNavigateMenu(MenuNavigation.Left);
    }

    private void NavigateDownPerformed(InputAction.CallbackContext obj)
    {
        SendNavigateMenu(MenuNavigation.Down);
    }

    private void NavigateUpPerformed(InputAction.CallbackContext obj)
    {
        SendNavigateMenu(MenuNavigation.Up);
    }

    private void NavigateForwardPerformed(InputAction.CallbackContext obj)
    {
        SendGoMenu();
    }

    private void NavigateBackPerformed(InputAction.CallbackContext obj)
    {
        SendBackMenu();
    }

    private void RemoveRaceListeners()
    {
        m_PlayerInput.actions["Accelerate"].performed -= AcceleratePerfomed;
        m_PlayerInput.actions["Accelerate"].canceled -= AcceleratePerfomed;
        m_PlayerInput.actions["Brake"].performed -= BrakePerformed;
        m_PlayerInput.actions["Brake"].canceled -= BrakePerformed;
        m_PlayerInput.actions["Steer"].performed -= SteerPerformed;
        m_PlayerInput.actions["Steer"].canceled -= SteerPerformed;
        m_PlayerInput.actions["Fire"].performed -= FirePerfomed;
        m_PlayerInput.actions["Fire"].canceled -= FireStoppedPerformed;
    }

    private void RemoveMenuListeners()
    {
        m_PlayerInput.actions["NavigateUp"].performed -= NavigateUpPerformed;
        m_PlayerInput.actions["NavigateDown"].performed -= NavigateDownPerformed;
        m_PlayerInput.actions["NavigateLeft"].performed -= NavigateLeftPerformed;
        m_PlayerInput.actions["NavigateRight"].performed -= NavigateRightPerformed;
        m_PlayerInput.actions["GoBackMenu"].performed -= NavigateBackPerformed;
        m_PlayerInput.actions["EnterMenu"].performed -= NavigateForwardPerformed;
    }

    private void OnDestroy()
    {
        GameManager.Current.GameStateChanged -= OnGameStateChanged;

        if (m_PlayerInput != null)
        {
            RemoveRaceListeners();
            RemoveMenuListeners();
        }
    }
}
