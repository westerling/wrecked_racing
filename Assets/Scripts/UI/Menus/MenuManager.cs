using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Current;

    private Menu m_ActiveMenu;

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        GameManager.Current.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    public void SetActiveMenu(Menu menu)
    {
        m_ActiveMenu = menu;
    }

    private void OnPlayerStatusChanged(Player player, bool status)
    {
        if (status)
        {
            PlayerJoined(player);

            return;
        }

        PlayerLeft(player);
    }

    private void PlayerJoined(Player playerJoining)
    {
        AddListeners(playerJoining);
    }

    private void AddListeners(Player playerJoining)
    {
        if (playerJoining.TryGetComponent(out InputManager inputManager))
        {
            inputManager.NavigateMenu += NavigateMenuPerformed;
            inputManager.GoMenu += GoMenuPerformed;
            inputManager.BackMenu += BackMenuPerformed;
        }
    }

    private void BackMenuPerformed(Player player)
    {
        m_ActiveMenu?.LeaveMenuPerformed(player);
    }

    private void GoMenuPerformed(Player player)
    {
        m_ActiveMenu?.EnterMenuPerformed(player);
    }

    private void NavigateMenuPerformed(Player player, MenuNavigation navigation)
    {
        switch (navigation)
        {
            case MenuNavigation.Up:
                m_ActiveMenu.NavigateUpPerformed(player);
                break;
            case MenuNavigation.Down:
                m_ActiveMenu.NavigateDownPerformed(player);
                break;
            case MenuNavigation.Left:
                m_ActiveMenu.NavigateLeftPerformed(player);
                break;
            case MenuNavigation.Right:
                m_ActiveMenu.NavigateRightPerformed(player);
                break;
        }
    }

    private void PlayerLeft(Player playerLeaving)
    {
        RemoveListeners(playerLeaving);
    }

    private void RemoveListeners(Player playerLeaving)
    {
        if (playerLeaving.TryGetComponent(out InputManager inputManager))
        {
            inputManager.NavigateMenu -= NavigateMenuPerformed;
            inputManager.GoMenu -= GoMenuPerformed;
            inputManager.BackMenu -= BackMenuPerformed;
        }
    }

    private void OnDestroy()
    {
        GameManager.Current.PlayerStatusChanged -= OnPlayerStatusChanged;

        foreach (var player in GameManager.Current.Players)
        {
            if (player == null)
            {
                continue;
            }

            RemoveListeners(player);
        }
    }
}
