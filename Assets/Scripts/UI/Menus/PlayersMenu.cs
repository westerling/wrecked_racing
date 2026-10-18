using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayersMenu : Menu
{
    [SerializeField]
    private PlayerCardsManager m_PlayerCardsManager;

    [SerializeField]
    private Menu m_RaceSettingsMenu;

    private List<Player> m_ReadyPlayers = new List<Player>();

    protected override void OnEnable()
    {
        base.OnEnable();

        m_ReadyPlayers.Clear();
        m_PlayerCardsManager.RemoveAllPlayerCards();

        AddPlayerCards();
    }

    private void AddPlayerCards()
    {
        foreach (var player in GameManager.Current.Players)
        {
            m_PlayerCardsManager.TryAddPlayerCard(player.InputType);
        }
    }

    public override void EnterMenuPerformed(Player player)
    {
        if (m_PlayerCardsManager.PlayerCardExists(player))
        {
            if (m_PlayerCardsManager.IsPlayerReady(player))
            {
                if (m_PlayerCardsManager.AllPlayersReady())
                {
                    StartRace();
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (m_PlayerCardsManager.TrySetPlayerReady(player))
                {
                    m_ReadyPlayers.Add(player);

                    if (m_PlayerCardsManager.AllPlayersReady())
                    {
                        StartRace();
                    }
                }
            }
        }
        else
        {
            if (m_PlayerCardsManager.FreePlayerCardExists())
            {
                if (!m_PlayerCardsManager.TryAddPlayer(player))
                {
                    Debug.LogError("Player could not be added.");
                }
            }
            else
            {
                if (!m_PlayerCardsManager.TryAddPlayerCard(player.InputType))
                {
                    Debug.LogError("No more player cards.");
                }
            }
        }           
    }

    public override void LeaveMenuPerformed(Player player)
    {
        if (m_PlayerCardsManager.PlayerCardExists(player))
        {
            if (m_PlayerCardsManager.IsPlayerReady(player))
            {

                if (m_ReadyPlayers.Contains(player))
                {
                    m_ReadyPlayers.Remove(player);
                }

                m_PlayerCardsManager.SetPlayerNotReady(player);
            }
            else
            {
                m_PlayerCardsManager.RemovePlayer(player);
            }
        }
        else
        {
            MenuManager.Current.PopMenu(this);
            return;
        }
    }

    public override void NavigateDownPerformed(Player player)
    {
    }

    public override void NavigateLeftPerformed(Player player)
    {
        if (m_PlayerCardsManager.PlayerCardExists(player))
        {
            if (!m_PlayerCardsManager.IsPlayerReady(player))
            {
                m_PlayerCardsManager.SetPreviousColor(player);
            }
        }
    }

    public override void NavigateRightPerformed(Player player)
    {
        if (m_PlayerCardsManager.PlayerCardExists(player))
        {
            if (!m_PlayerCardsManager.IsPlayerReady(player))
            {
                m_PlayerCardsManager.SetNextColor(player);
            }
        }
    }

    public override void NavigateUpPerformed(Player player)
    {
    }

    protected override void OnPlayerJoined(Player playerJoining)
    {
        if (m_ReadyPlayers.Contains(playerJoining))
        {
            m_ReadyPlayers.Remove(playerJoining);
        }

        m_PlayerCardsManager.TryAddPlayerCard(playerJoining.InputType);
    }

    protected override void OnPlayerLeft(Player playerLeaving)
    {
        if (m_ReadyPlayers.Contains(playerLeaving))
        {
            m_ReadyPlayers.Remove(playerLeaving);
        }

        m_PlayerCardsManager.RemovePlayer(playerLeaving);
        m_PlayerCardsManager.RemovePlayerCard(playerLeaving);
    }

    protected override void OnPlayerStatusChanged(Player player, bool playerStatus)
    {
    }

    private void StartRace()
    {
        GameManager.Current.SetActivePlayers(m_ReadyPlayers);
        MenuManager.Current.AddMenu(m_RaceSettingsMenu, this);
    }
}
