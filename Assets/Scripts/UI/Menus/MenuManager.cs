using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Current;

    private List<Menu> m_Menus = new List<Menu>();

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        GameManager.Current.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    public void AddMenu(Menu menu)
    {
        menu.gameObject.SetActive(true);

        m_Menus.Add(menu);
        SetActiveGameObject();
    }

    public void AddMenu(Menu menu, Menu parent)
    {
        parent.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);

        m_Menus.Add(menu);
        SetActiveGameObject();
    }

    public void PopMenu(Menu menu)
    {
        if (TryGetActiveMenu(out var oldMenu))
        {
            oldMenu.gameObject.SetActive(false);

            if (m_Menus.Contains(menu))
            {
                m_Menus.Remove(menu);
            }
        }

        if (m_Menus.Any())
        {
            var parentMenu = m_Menus.Last();
            parentMenu.gameObject.SetActive(true);

            SetActiveGameObject();
        }
    }

    private void SetActiveGameObject()
    {
        if (TryGetActiveMenu(out var menu))
        {
            if (menu is ButtonsMenu buttonsMenu)
            {
                buttonsMenu.EventSystem.SetSelectedGameObject(buttonsMenu.StartSelection.gameObject);
            }
        }
    }

    private bool TryGetActiveMenu(out Menu menu)
    {
        if (m_Menus.Any())
        {
            menu = m_Menus.Last();
            return true;
        }

        menu = null;
        return false;
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
        m_Menus.Last()?.LeaveMenuPerformed(player);
    }

    private void GoMenuPerformed(Player player)
    {
        m_Menus.Last()?.EnterMenuPerformed(player);
    }

    private void NavigateMenuPerformed(Player player, MenuNavigation navigation)
    {
        switch (navigation)
        {
            case MenuNavigation.Up:
                m_Menus.Last()?.NavigateUpPerformed(player);
                break;
            case MenuNavigation.Down:
                m_Menus.Last()?.NavigateDownPerformed(player);
                break;
            case MenuNavigation.Left:
                m_Menus.Last()?.NavigateLeftPerformed(player);
                break;
            case MenuNavigation.Right:
                m_Menus.Last()?.NavigateRightPerformed(player);
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
