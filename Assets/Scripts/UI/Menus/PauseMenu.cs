public class PauseMenu : Menu
{
    private void Start()
    {
        MenuManager.Current.AddMenu(this);
    }

    public void Resume()
    {

    }
    
    public void Exit()
    {
        RaceManager.Current.Quit();
    }

    public override void EnterMenuPerformed(Player player)
    {
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
