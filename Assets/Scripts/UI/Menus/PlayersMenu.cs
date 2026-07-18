using TMPro;
using UnityEngine;

public class PlayersMenu : Menu
{
    [SerializeField]
    private PlayerCardsManager m_PlayerCardsManager;

    [SerializeField]
    private Menu m_RaceSettingsMenu;

    [SerializeField]
    private TMP_Text m_StatusText;

    protected override void OnEnable()
    {
        base.OnEnable();

        UpdateText();
        m_PlayerCardsManager.ResetPlayerCards();
    }

    public override void EnterMenuPerformed(Player player)
    {
        if (m_PlayerCardsManager.IsPlayerAdded(player))
        {
            if (m_PlayerCardsManager.IsPlayerReady(player))
            {
                if (m_PlayerCardsManager.AllPlayersReady())
                {
                    MenuManager.Current.AddMenu(m_RaceSettingsMenu, this);
                }
            }
            else
            {
                m_PlayerCardsManager.TrySetPlayerReady(player);
            }
        }
        else
        {
            var playerAdded = m_PlayerCardsManager.TryAddPlayer(player);

            if (!playerAdded)
            {
                Debug.LogError("Player could not be added.");
            }
        }

        UpdateText();
    }

    public override void LeaveMenuPerformed(Player player)
    {
        if (m_PlayerCardsManager.IsPlayerAdded(player))
        {
            if (m_PlayerCardsManager.IsPlayerReady(player))
            {
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

        UpdateText();
    }

    public override void NavigateDownPerformed(Player player)
    {
    }

    public override void NavigateLeftPerformed(Player player)
    {
        if (m_PlayerCardsManager.IsPlayerAdded(player))
        {
            if (!m_PlayerCardsManager.IsPlayerReady(player))
            {
                m_PlayerCardsManager.SetPreviousColor(player);
            }
        }
    }

    public override void NavigateRightPerformed(Player player)
    {
        if (m_PlayerCardsManager.IsPlayerAdded(player))
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
    }

    protected override void OnPlayerLeft(Player playerLeaving)
    {
    }

    protected override void OnPlayerStatusChanged(Player player, bool playerStatus)
    {
    }

    private void UpdateText()
    {
    }
}
