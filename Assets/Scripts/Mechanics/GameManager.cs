using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Current;

    public event Action<Player, bool> PlayerStatusChanged;

    [SerializeField]
    private GameObject m_Camera;

    private List<Player> m_Players;
    private RaceSettings m_RaceSettings;

    public List<Player> Players
    {
        get => m_Players;
        private set => m_Players = value;
    }

    public RaceSettings RaceSettings
    {
        get => m_RaceSettings;
        private set => m_RaceSettings = value;
    }

    public GameObject Camera 
    {
        get => m_Camera;
    }
    
    private void Awake()
    {
        Current = this;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerInput.TryGetComponent(out Player player))
        {
            if (!Players.Contains(player))
            {
                Players.Add(player);
            }
        }
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {
        if (playerInput.TryGetComponent(out Player player))
        {
            if (Players.Contains(player))
            {
                Players.Remove(player);
            }
        }
    }
}
