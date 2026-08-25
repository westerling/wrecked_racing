using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaceSettingsMenu : ButtonsMenu
{
    [Header("UI")]
    [SerializeField]
    private Image m_TrackImage;

    [SerializeField]
    private TMP_Text m_TrackDescription; 

    [SerializeField]
    private Image m_CarImage;

    private int m_TrackIndex = 0;
    private int m_CarIndex = 0;
    private int m_RaceModeIndex = 0;
    private int m_PowerupsIndex = 0;
    private bool m_IsAirstrikeOn = true;
    private bool m_IsPowerupsOn = true;

    private TrackInfo m_SelectedTrackInfo;
    private GameObject m_SelectedCar;

    protected override void OnEnable()
    {
        base.OnEnable();

        UpdateInformation();
    }

    public override void EnterMenuPerformed(Player player)
    {
        if (EventSystem.currentSelectedGameObject.TryGetComponent(out CustomButton button))
        {
            button.ButtonSelected();
        }
    }

    public override void LeaveMenuPerformed(Player player)
    {
        MenuManager.Current.PopMenu(this);
    }

    public override void NavigateDownPerformed(Player player)
    {
    }

    public override void NavigateLeftPerformed(Player player)
    {
        if (EventSystem.currentSelectedGameObject.TryGetComponent(out SelectionButton selectionButton))
        {
            selectionButton.OnLeft();
        }
    }

    public override void NavigateRightPerformed(Player player)
    {
        if (EventSystem.currentSelectedGameObject.TryGetComponent(out SelectionButton selectionButton))
        {
            selectionButton.OnRight();
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

    public void StartRace()
    {
        var raceSettings = new RaceSettings
        {
            Car = m_SelectedCar,
            SceneIndex = m_SelectedTrackInfo.SceneIndex,
        };

        GameManager.Current.LoadTrack(raceSettings);
    }

    public void NextTrack()
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

    public void PreviousTrack()
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

    public void NextCar()
    {
        if (GameManager.Current.Cars.Count > m_CarIndex + 1)
        {
            m_CarIndex++;
        }
        else
        {
            m_CarIndex = 0;
        }

        UpdateInformation();
    }

    public void PreviousCar()
    {
        if (m_CarIndex <= 0)
        {
            m_CarIndex = GameManager.Current.Cars.Count - 1;
        }
        else
        {
            m_CarIndex--;
        }

        UpdateInformation();
    }

    public void TogglePowerups()
    {
        m_IsPowerupsOn= !m_IsPowerupsOn;

        UpdateInformation();
    }

    public void ToggleAirstrike()
    {
        m_IsAirstrikeOn = !m_IsAirstrikeOn;

        UpdateInformation();
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
    }

    private void UpdateUI()
    {
        m_TrackImage.sprite = m_SelectedTrackInfo.TrackImage;
        CreateDescription();

        if (m_SelectedCar.TryGetComponent(out Car car))
        {
            m_CarImage.sprite = car.Stats.Image;
        }
    }

    private void CreateDescription()
    {
        var airstrikeText = m_IsAirstrikeOn ? "ON" : "OFF";
        var powerupText = m_IsPowerupsOn ? "ON" : "OFF";

        m_TrackDescription.text =
            m_SelectedTrackInfo.TrackDescription + "\n"
            + "Airstrike: " + airstrikeText + "\n"
            + "Powerups: " + powerupText;
    }
}