using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Menu : MonoBehaviour
{    
    [SerializeField]
    private AudioClip[] m_MenuMusic;

    private EventSystem m_EventSystem;

    public EventSystem EventSystem => m_EventSystem != null
                                  ? m_EventSystem
                                  : EventSystem.current;

    protected virtual void OnEnable()
    {
        m_EventSystem = EventSystem.current;
    }

    private void Start()
    {
        MusicManager.Current.AddToQueue(m_MenuMusic, true);
        MusicManager.Current.Play();
    }

    public abstract void EnterMenuPerformed(Player player);
    public abstract void LeaveMenuPerformed(Player player);

    public abstract void NavigateUpPerformed(Player player);

    public abstract void NavigateDownPerformed(Player player);

    public abstract void NavigateLeftPerformed(Player player);

    public abstract void NavigateRightPerformed(Player player);

    protected abstract void OnPlayerStatusChanged(Player player, bool playerStatus);

    protected abstract void OnPlayerJoined(Player playerJoining);

    protected abstract void OnPlayerLeft(Player playerLeaving);
}
