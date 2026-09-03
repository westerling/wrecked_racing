using UnityEngine;
using UnityEngine.UI;

public class MainMenu : ButtonsMenu
{
    [SerializeField]
    private Menu m_StartGameMenu;

    [SerializeField]
    private Menu m_SettingsMenu;

    private void Start()
    {
        MenuManager.Current.AddMenu(this);
    }

    public void OpenPlayerSettingsMenu()
    {
        MenuManager.Current.AddMenu(m_StartGameMenu, this);
    }

    public void OpenSettingsMenu()
    {
        MenuManager.Current.AddMenu(m_SettingsMenu, this);
    }

    public void ExitGame()
    {
        Application.Quit();
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
