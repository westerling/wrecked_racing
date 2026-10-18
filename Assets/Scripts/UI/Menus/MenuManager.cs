using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Current;

    [SerializeField]
    private Menu m_MainMenu;

    [SerializeField]
    private List<Menu> m_Menus = new();

    private readonly List<Menu> m_ActiveMenus = new();

    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        GameManager.Current.PlayerStatusChanged += OnPlayerStatusChanged;
    }

    public void ResetMenuLayout()
    {
        AddMenu(MenuType.MainMenu, MenuLoadMode.Single);
    }

    public void AddMenu(MenuType menuType, MenuLoadMode loadMode)
    {
        if (loadMode == MenuLoadMode.Single)
        {
            m_ActiveMenus.Clear();
        }

        var menu = m_Menus.FirstOrDefault(x => x.MenuType == menuType);

        if (menu == null)
        {
            Debug.LogError("No Menu of type: " + menuType + " exists.");
            return;
        }

        menu.gameObject.SetActive(true);
        m_ActiveMenus.Add(menu);
        SetActiveSelectedMenuItem();
    }

    public void AddMenu(MenuType menuType, Menu parent, MenuLoadMode loadMode)
    {
        if (loadMode == MenuLoadMode.Single)
        {
            m_ActiveMenus.Clear();
        }

        var menu = m_Menus.FirstOrDefault(x => x.MenuType == menuType);

        if (menu == null)
        {
            Debug.LogError("No Menu of type: " + menuType + " exists.");
            return;
        }

        parent.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);
        m_ActiveMenus.Add(menu);
        SetActiveSelectedMenuItem();
    }

    public void AddMenu(Menu menu)
    {
        menu.gameObject.SetActive(true);

        m_ActiveMenus.Add(menu);
        SetActiveSelectedMenuItem();
    }

    public void AddMenu(Menu menu, Menu parent)
    {
        parent.gameObject.SetActive(false);
        menu.gameObject.SetActive(true);

        m_ActiveMenus.Add(menu);
        SetActiveSelectedMenuItem();
    }

    public void CleanMenus()
    {
        foreach (var menu in m_ActiveMenus)
        {
            menu.gameObject.SetActive(false);
        }

        m_ActiveMenus.Clear();
    }

    public void PopMenu(Menu menu)
    {
        if (TryGetActiveMenu(out var oldMenu))
        {
            oldMenu.gameObject.SetActive(false);

            if (m_ActiveMenus.Contains(menu))
            {
                m_ActiveMenus.Remove(menu);
            }
        }

        if (m_ActiveMenus.Any())
        {
            var parentMenu = m_ActiveMenus.Last();
            parentMenu.gameObject.SetActive(true);

            SetActiveSelectedMenuItem();
        }
    }

    private void SetActiveSelectedMenuItem()
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
        if (m_ActiveMenus.Any())
        {
            menu = m_ActiveMenus.Last();
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
        m_ActiveMenus.Last()?.LeaveMenuPerformed(player);
    }

    private void GoMenuPerformed(Player player)
    {
        m_ActiveMenus.Last()?.EnterMenuPerformed(player);
    }

    private void NavigateMenuPerformed(Player player, MenuNavigation navigation)
    {
        switch (navigation)
        {
            case MenuNavigation.Up:
                m_ActiveMenus.Last()?.NavigateUpPerformed(player);
                break;
            case MenuNavigation.Down:
                m_ActiveMenus.Last()?.NavigateDownPerformed(player);
                break;
            case MenuNavigation.Left:
                m_ActiveMenus.Last()?.NavigateLeftPerformed(player);
                break;
            case MenuNavigation.Right:
                m_ActiveMenus.Last()?.NavigateRightPerformed(player);
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
