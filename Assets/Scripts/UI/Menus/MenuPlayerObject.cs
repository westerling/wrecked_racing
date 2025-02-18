using UnityEngine;

public class MenuPlayerObject : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_ReadyGameObjects;

    [SerializeField]
    private GameObject[] m_NotReadyGameObjects;

    private Player m_Player;

    public Player Player
    {
        get => m_Player;
        set => m_Player = value;
    }

    private void Awake()
    {
        SetStatus(PlayerStatus.Inactive);
    }

    public void SetStatus(PlayerStatus status)
    {
        switch (status)
        {
            case PlayerStatus.Inactive:
                ToggleObjects(m_ReadyGameObjects, false);
                ToggleObjects(m_NotReadyGameObjects, true);
                break;
            case PlayerStatus.Active:
                ToggleObjects(m_ReadyGameObjects, true);
                ToggleObjects(m_NotReadyGameObjects, false);
                break;
            default:
                ToggleObjects(m_ReadyGameObjects, false);
                ToggleObjects(m_NotReadyGameObjects, true);
                break;
        }
    }

    private void ToggleObjects(GameObject[] gameObjects, bool active)
    {
        foreach (var go in gameObjects)
        {
            go.SetActive(active);
        }
    }
}
