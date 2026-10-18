using UnityEngine;

public class PauseMenu : ButtonsMenu
{
    public void Resume()
    {
        GameManager.Current.SetGameState(GameState.Race);
        RaceManager.Current.PauseTime(false);
        MenuManager.Current.CleanMenus();
    }
    
    public void Exit()
    {
        RaceManager.Current.Quit();
    }

    public void OpenMenu(MenuType menuType)
    {
        MenuManager.Current.AddMenu(menuType, this, MenuLoadMode.Additive);
    }

    public void OpenSettingsMenu()
    {
        MenuManager.Current.AddMenu(MenuType.PauseSettingsMenu, this, MenuLoadMode.Additive);
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
    }

    public override void NavigateDownPerformed(Player player)
    {
    }

    public override void NavigateLeftPerformed(Player player)
    {
    }

    public override void NavigateRightPerformed(Player player)
    {
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
}
