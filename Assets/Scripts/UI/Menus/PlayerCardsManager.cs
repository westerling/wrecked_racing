using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCardsManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerCard> m_PlayerCards = new List<PlayerCard>();

    [SerializeField]
    private List<PlayerCardSprite> m_PlayerCardSprites = new List<PlayerCardSprite>();

    public void ResetPlayerCards()
    {
        foreach (var playerCard in m_PlayerCards)
        {
            playerCard.Player = null;
            playerCard.Ready = false;
            playerCard.Color = PlayerColor.Black;
            playerCard.InputImageSelector.SetSpriteInvisible();
            playerCard.Image.sprite = m_PlayerCardSprites.First().Open;
        }
    }

    public bool TryAddPlayer(Player player)
    {
        if (m_PlayerCards.Where(x => x.Player == null).Any())
        {
            if (m_PlayerCards.Any(x => x.Player == player))
            {
                return false;
            }
            else
            {
                var playerCard = m_PlayerCards.First(x => x.Player == null);
                var color = GetFirstFreeColor();

                playerCard.Player = player;
                playerCard.Ready = false;
                playerCard.Color = color;
                playerCard.InputImageSelector.SetSpriteVisible(player.InputType);
                playerCard.Image.sprite = GetSpriteFromColor(color, false);

                return true;
            }
        }

        return false;
    }

    private Sprite GetSpriteFromColor(PlayerColor color, bool isReady)
    {
        var playerCardSprite = m_PlayerCardSprites.Where(x => x.PlayerColor == color).First();

        return isReady ? playerCardSprite.PlayerReady : playerCardSprite.PlayerNotReady;
    }

    private Sprite GetBlockedSpriteFromColor(PlayerColor color)
    {
        var playerCardSprite = m_PlayerCardSprites.Where(x => x.PlayerColor == color).First();

        return playerCardSprite.Locked;
    }

    public void RemovePlayer(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard != null)
        {
            playerCard.Player = null;
            playerCard.Ready = false;
            playerCard.InputImageSelector.SetSpriteInvisible();
            playerCard.Image.sprite = m_PlayerCardSprites.First().Open;
        }
    }

    public bool IsPlayerAdded(Player player)
    {
        if (m_PlayerCards.Any(x => x.Player == player))
        {
            return true;
        }

        return false;
    }

    public bool IsPlayerReady(Player player)
    {
        if (m_PlayerCards.Any(x => x.Player == player && x.Ready))
        {
            return true;
        }

        return false;
    }

    public bool AllPlayersReady()
    {
        return true;
    }

    public void TrySetPlayerReady(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard != null)
        {
            if (IsColorTaken(player, playerCard.Color))
            {
                return;
            }

            playerCard.Ready = true;
            playerCard.Image.sprite = GetSpriteFromColor(playerCard.Color, true);
            player.Color = playerCard.Color;

            BlockOtherPlayers(playerCard);
        }
    }

    public void SetPlayerNotReady(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard != null)
        {
            playerCard.Ready = false;
            playerCard.Image.sprite = GetSpriteFromColor(playerCard.Color, false);
        }

        UnlockOtherPlayers(playerCard);
    }

    private void UnlockOtherPlayers(PlayerCard playerPlayerCard)
    {
        foreach (var playerCard in m_PlayerCards)
        {
            if (playerCard.Player == null || playerCard == playerPlayerCard)
            {
                continue;
            }

            if (playerCard.Color == playerPlayerCard.Color)
            {
                if (!playerCard.Ready)
                {
                    playerCard.Image.sprite = GetSpriteFromColor(playerCard.Color, false);
                }
            }
        }
    }

    private void BlockOtherPlayers(PlayerCard playerPlayerCard)
    {
        foreach (var playerCard in m_PlayerCards)
        {
            if (playerCard.Player == null || playerCard == playerPlayerCard)
            {
                continue;
            }

            if (playerCard.Color == playerPlayerCard.Color)
            {
                if (!playerCard.Ready)
                {
                    Debug.Log("Block!!!");
                    playerCard.Image.sprite = GetBlockedSpriteFromColor(playerCard.Color);
                }
            }
        }
    }

    private PlayerColor GetFirstFreeColor()
    {
        var colors = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));

        for (var i = 0; i < colors.Length; i++)
        {
            var color = colors[i];

            if (m_PlayerCards.Where(x => x.Player != null && x.Color == color).Any())
            {
                continue;
            }
            else
            {
                return colors[i];
            }
        }

        return PlayerColor.Black;
    }

    public void SetNextColor(Player player)
    {
        if (IsPlayerReady(player))
        {
            return;
        }

        if (IsPlayerAdded(player))
        {
            var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);
            var colors = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));
            var currentColor = GetPlayerColor(player);
            var index = Array.IndexOf(colors, currentColor);

            index++;

            if (index > colors.Length-1)
            {
                index = 0;
            }

            var color = colors[index];

            if (playerCard != null)
            {
                playerCard.Color = color;
                playerCard.Image.sprite = IsColorTaken(player, color)
                    ? GetBlockedSpriteFromColor(color)
                    : GetSpriteFromColor(color, false);
            }
        }
    }

    public void SetPreviousColor(Player player)
    {
        if (IsPlayerReady(player))
        {
            return;
        }

        if (IsPlayerAdded(player))
        {
            var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);
            var colors = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));
            var currentColor = GetPlayerColor(player);
            var index = Array.IndexOf(colors, currentColor);

            index--;

            if (index < 0)
            {
                index = colors.Length-1;
            }

            var color = colors[index];

            if (playerCard != null)
            {
                playerCard.Color = color;
                playerCard.Image.sprite = IsColorTaken(player, color) 
                    ? GetBlockedSpriteFromColor(color) 
                    : GetSpriteFromColor(color, false);
            }
        }
    }

    private bool IsColorTaken(Player player, PlayerColor color)
    {
        foreach (var playerCard in m_PlayerCards)
        {
            if (playerCard.Player == player)
            {
                continue;
            }

            if (playerCard.Color == color && playerCard.Ready)
            {
                return true;
            }
        }

        return false;
    }

    private PlayerColor GetPlayerColor(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard == null)
        {
            return PlayerColor.Black;
        }

        return playerCard.Color;
    }
}
