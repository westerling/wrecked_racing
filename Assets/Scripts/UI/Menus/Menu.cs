using UnityEngine;

public abstract class Menu : MonoBehaviour
{
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
