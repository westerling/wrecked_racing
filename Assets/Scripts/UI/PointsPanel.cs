using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointsPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_PointsText;

    [SerializeField]
    private GameObject m_PointsMarkersPanel;

    [SerializeField]
    private Image m_CarIcon;

    [SerializeField]
    private PointMarker[] m_PointMarkers;

    [SerializeField]
    private GameObject m_WinIcon;

    private int m_Points;
    private int m_NumberOfPlayers;

    private PlayerCar m_Car;

    public PlayerCar Car
    {
        get => m_Car;
        private set => m_Car = value;
    }

    private void Start()
    {
        SetWidth();
    }

    public void AddCar(PlayerCar car, int startPoints)
    {
        Car = car;
        m_Points = startPoints;
        UpdatePoints();

        SetCarPanelBackground(Globals.GetPlayerColor(Car.Player.Color, 200));

        m_CarIcon.sprite = Car.Stats.Image;
        m_WinIcon.SetActive(false);
        m_NumberOfPlayers = RaceManager.Current.Cars.Count;
    }

    public void SetNewPoints(int newPoints)
    {
        m_PointsText.text = FormatText(newPoints);
        m_Points += newPoints;
        
        UpdatePoints();
        UptadeWinIcon();
    }

    public void SetCarPanel(bool active)
    {
        if (active)
        {
            SetCarPanelBackground(Globals.GetPlayerColor(Car.Player.Color, 200));
        }
        else
        {
            SetCarPanelBackground(new Color32(20,20,20, 150));
        }
    }

    private void SetCarPanelBackground(Color color)
    {
        if (gameObject.TryGetComponent(out Image image))
        {
            image.color = color;
        }
    }

    private void UpdatePoints()
    {
        for (var i = 0; i < m_PointMarkers.Length; i++)
        {
            m_PointMarkers[i].SetPointImageActive(m_Points > i);
        }
    }

    private void UptadeWinIcon()
    {
        if (m_NumberOfPlayers == 4)
        {
            m_WinIcon.SetActive(m_Points >= 10);
        }

        else
        {
            m_WinIcon.SetActive(m_Points >= 7);
        }
    }

    private void SetWidth()
    {
        var maxPoints = Globals.MaxPoints(m_NumberOfPlayers);

        for (var i = 0; i < m_PointMarkers.Length; i++)
        {
            m_PointMarkers[i].gameObject.SetActive(maxPoints > i);
        }
    }

    private string FormatText(int newPoints)
    {
        if (newPoints >= 0)
        {
            return "+" + newPoints;
        }
        
        return newPoints.ToString();
    }
}
