using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GarageMenu : Menu
{
    [Header("UI")]
    [SerializeField]
    private Image m_TrackImage;

    [SerializeField]
    private List<MenuPlayerObject> m_GaragePlayers = new List<MenuPlayerObject>();

    private GameObject m_SelectedCar;
    private TrackInfo m_SelectedTrackInfo;
    private List<Player> m_ActivePlayers = new List<Player>();
    
    private int m_TrackIndex = 0;
    private int m_CarIndex = 0;
    private int m_BotsIndex = 0;

    private void Start()
    {
        MenuManager.Current.SetActiveMenu(this);

        UpdateInformation();
    }

    public override void EnterMenuPerformed(Player player)
    {
        if (m_ActivePlayers.Contains(player))
        {
            GameManager.Current.SetActivePlayers(m_ActivePlayers);
            StartRace();
        }
        else
        {
            AddNewPlayer(player);
            CheckPlayerStatus();
        }

        UpdateInformation();
    }

    public override void LeaveMenuPerformed(Player player)
    {
        if (m_ActivePlayers.Contains(player))
        {
            RemovePlayer(player);
        }

        CheckPlayerStatus();
        UpdateInformation();
    }

    public override void NavigateDownPerformed(Player player)
    {
        if (m_ActivePlayers.Contains(player))
        {
            NextTrack();
        }
    }

    public override void NavigateLeftPerformed(Player player)
    {
        return;
    }

    public override void NavigateRightPerformed(Player player)
    {
        return;
    }

    public override void NavigateUpPerformed(Player player)
    {
        if (m_ActivePlayers.Contains(player))
        {
            PreviousTrack();
        }
    }

    protected override void OnPlayerJoined(Player playerJoining)
    {
        CheckPlayerStatus();
    }

    protected override void OnPlayerLeft(Player playerLeaving)
    {
        RemovePlayer(playerLeaving);
        CheckPlayerStatus();
    }

    protected override void OnPlayerStatusChanged(Player player, bool playerStatus)
    {
        return;
    }

    private void AddNewPlayer(Player newPlayer)
    {
        var inactivePlayerPanels = m_GaragePlayers.Where(x => x.Player == null).ToList();

        if (!inactivePlayerPanels.Any())
        {
            return;
        }

        inactivePlayerPanels.First().Player = newPlayer;
        
        m_ActivePlayers.Add(newPlayer);
    }

    private void RemovePlayer(Player player)
    {
        if (m_ActivePlayers.Contains(player))
        {
            m_ActivePlayers.Remove(player);
        }

        if (m_GaragePlayers.Any(x => x.Player == player))
        {
            var playerCard = m_GaragePlayers.First(x => x.Player == player);

            playerCard.Player = null;
        }
    }

    private void CheckPlayerStatus()
    {
        foreach (var playerPanel in m_GaragePlayers)
        {
            if (playerPanel.Player != null)
            {
                playerPanel.SetStatus(PlayerStatus.Active);
            }
            else
            {
                playerPanel.SetStatus(PlayerStatus.Inactive);
            }
        }
    }

    private void NextTrack()
    {
        if (GameManager.Current.Tracks.Count > m_TrackIndex + 1)
        {
            m_TrackIndex++;
        }
        else
        {
            m_TrackIndex = 0;
        }

        UpdateInformation();
    }

    private void PreviousTrack()
    {
        if (m_TrackIndex <= 0)
        {
            m_TrackIndex = GameManager.Current.Tracks.Count - 1;
        }
        else
        {
            m_TrackIndex--;
        }

        UpdateInformation();
    }

    private void StartRace()
    {
        var raceSettings = new RaceSettings
        {
            Car = m_SelectedCar,
            SceneIndex = m_SelectedTrackInfo.SceneIndex,
            NumberOfBots = m_BotsIndex
        };

        GameManager.Current.LoadTrack(raceSettings);
    }

    private void UpdateInformation()
    {
        GetInformation();
        UpdateUI();
    }

    private void GetInformation()
    {
        m_SelectedTrackInfo = GameManager.Current.Tracks[m_TrackIndex];
        m_SelectedCar = GameManager.Current.Cars[m_CarIndex];
        m_BotsIndex = 0;
    }

    private void UpdateUI()
    {
        m_TrackImage.gameObject.SetActive(m_ActivePlayers.Count > 0);
        m_TrackImage.sprite = m_SelectedTrackInfo.TrackImage;
    }
}
