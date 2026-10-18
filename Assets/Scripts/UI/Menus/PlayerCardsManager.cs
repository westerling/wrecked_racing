using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCardsManager : MonoBehaviour
{
    [SerializeField]
    private PlayerCard m_PlayerCard;

    [SerializeField]
    private List<PlayerCardSprite> m_PlayerCardSprites = new List<PlayerCardSprite>();

    [SerializeField]
    private Sprite m_LockedImage;

    [SerializeField]
    private Color m_DefaultColor;

    [SerializeField]
    private Sprite m_DefaultImage;

    private List<PlayerCard> m_PlayerCards = new List<PlayerCard>();
    private List<Player> m_ActivePlayers = new List<Player>();
                
    public bool TryAddPlayerCard(InputType inputType)
    {
        if (m_PlayerCards.Count < 4)
        {
            var playerCardGameObject = Instantiate(m_PlayerCard, transform);

            SetDefaultValues(playerCardGameObject, inputType);
            m_PlayerCards.Add(playerCardGameObject);

            return true;
        }

        return false;
    }

    public void RemoveAllPlayerCards()
    {
        var playerCards = transform.GetComponentsInChildren<PlayerCard>().ToList();

        for (var i = playerCards.Count - 1; i >= 0; i--)
        {
            Destroy(playerCards[i].gameObject);
        }

        m_PlayerCards.Clear();
        m_ActivePlayers.Clear();
    }

    public void RemovePlayer(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard != null)
        {
            SetDefaultValues(playerCard, player.InputType);
        }
    }

    public void RemovePlayerCard(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard != null)
        {
            m_PlayerCards.Remove(playerCard);
            Destroy(playerCard.gameObject);
        }
    }

    public bool TryAddPlayer(Player player)
    {
        if (m_ActivePlayers.Contains(player))
        {
            Debug.Log("player already active");
            return false;
        }

        if (m_PlayerCards.Any(x => x.Player == null))
        {
            var playerCard = m_PlayerCards.First(x => x.Player == null);
            var color = GetFirstFreeColor();
            var playerCardSprite = GetPlayerCardSpriteFromColor(color);

            playerCard.Player = player;
            playerCard.Ready = false;
            playerCard.Color = color;
            playerCard.ColorCarousel.SetActive(true);
            playerCard.ReadyText.text = string.Empty;
            playerCard.StatusText.text = string.Empty;
            playerCard.ColorText.text = color.ToString();
            playerCard.Icon.sprite = playerCardSprite.Icon;
            playerCard.Icon.color = new Color(255, 255, 255, 255);
            playerCard.ColorImage.sprite = playerCardSprite.BackgroundImage;
            playerCard.ColorImage.color = playerCardSprite.Color;

            ToggleIconsFromInput(playerCard, player.InputType);

            return true;
        }
        else
        {
            Debug.Log("No free cards");
        }

        return false;
    }

    public bool TrySetPlayerReady(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard != null)
        {
            if (!IsColorFree(player, playerCard.Color))
            {
                return false;
            }

            playerCard.Ready = true;
            playerCard.ReadyText.text = "Ready";
            playerCard.StatusText.text = "Waiting for other players...";
            player.Color = playerCard.Color;

            BlockOtherPlayers(playerCard);

            return true;
        }

        return false;
    }

    public bool IsGameReadyToStart()
    {
        return m_ActivePlayers.Any();
    }

    public bool FreePlayerCardExists()
    {
        if (m_PlayerCards.Any(x => x.Player == null))
        {
            return true;
        }

        return false;
    }

    public bool PlayerCardExists(Player player)
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
        if (m_PlayerCards.Any(x => x.Player != null && x.Ready == false))
        {
            return false;
        }

        return true;
    }

    public void SetPlayerNotReady(Player player)
    {
        var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);

        if (playerCard != null)
        {
            playerCard.Ready = false;
            playerCard.StatusText.text = "Press A to Start";
            playerCard.ReadyText.text = string.Empty;
            playerCard.StatusText.text = string.Empty;
        }

        UnlockOtherPlayers(playerCard);
    }

    public void SetNextColor(Player player)
    {
        if (IsPlayerReady(player))
        {
            return;
        }

        if (PlayerCardExists(player))
        {
            var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);
            var colors = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));
            var currentColor = GetPlayerColor(player);
            var index = Array.IndexOf(colors, currentColor);

            index++;

            if (index > colors.Length - 1)
            {
                index = 0;
            }

            var color = colors[index];

            if (playerCard != null)
            {
                UpdatePlayerCard(playerCard, player, color);
            }
        }
    }

    public void SetPreviousColor(Player player)
    {
        if (IsPlayerReady(player))
        {
            return;
        }

        if (PlayerCardExists(player))
        {
            var playerCard = m_PlayerCards.FirstOrDefault(x => x.Player == player);
            var colors = (PlayerColor[])Enum.GetValues(typeof(PlayerColor));
            var currentColor = GetPlayerColor(player);
            var index = Array.IndexOf(colors, currentColor);

            index--;

            if (index < 0)
            {
                index = colors.Length - 1;
            }

            var color = colors[index];

            if (playerCard != null)
            {
                UpdatePlayerCard(playerCard, player, color);
            }
        }
    }

    private void SetDefaultValues(PlayerCard playerCard, InputType inputType)
    {
        playerCard.Player = null;
        playerCard.Ready = false;
        playerCard.ReadyText.text = string.Empty;
        playerCard.StatusText.text = "Press A to join";
        playerCard.ColorImage.color = m_DefaultColor;
        playerCard.Icon.sprite = null;
        playerCard.Icon.color = new Color(255, 255, 255, 0);
        playerCard.ColorCarousel.SetActive(false);
        playerCard.ColorImage.sprite = m_DefaultImage;

        ToggleIconsFromInput(playerCard, inputType);
    }

    private void ToggleIconsFromInput(PlayerCard playerCard, InputType inputType)
    {
        foreach (var go in playerCard.KeyboardImages)
        {
            go.SetActive(inputType == InputType.Keyboard);
        }

        foreach (var go in playerCard.GamepadImages)
        {
            go.SetActive(inputType == InputType.Controller);
        }
    }

    private PlayerCardSprite GetPlayerCardSpriteFromColor(PlayerColor color)
    {
        var playerCardSprite = m_PlayerCardSprites.FirstOrDefault(x => x.PlayerColor == color);

        if (playerCardSprite != null)
        {
            return playerCardSprite;
        }

        Debug.LogError("No sprite seettings found");

        return null;
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
                    var playerCardSprite = GetPlayerCardSpriteFromColor(playerCard.Color);

                    playerCard.Icon.sprite = playerCardSprite.Icon;
                    playerCard.ColorImage.color = playerCardSprite.Color;
                    playerCard.ColorText.text = playerCard.Color.ToString();
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
                    var playerCardSprite = GetPlayerCardSpriteFromColor(playerCard.Color);

                    playerCard.Icon.sprite = m_LockedImage;
                    playerCard.ColorImage.color = playerCardSprite.ColorUnavailable;
                    playerCard.ColorText.text = "Taken";
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

    private void UpdatePlayerCard(PlayerCard playerCard, Player player, PlayerColor color)
    {
        var playerCardSprite = GetPlayerCardSpriteFromColor(color);

        playerCard.Color = color;
        playerCard.Icon.sprite = IsColorFree(player, color)
            ? playerCardSprite.Icon
            : m_LockedImage;

        playerCard.ColorImage.color = IsColorFree(player, color)
            ? playerCardSprite.Color
            : playerCardSprite.ColorUnavailable;


        playerCard.ColorText.text = IsColorFree(player, color)
            ? color.ToString()
            : "Taken";
    }

    private bool IsColorFree(Player player, PlayerColor color)
    {
        foreach (var playerCard in m_PlayerCards)
        {
            if (playerCard.Player == player)
            {
                continue;
            }

            if (playerCard.Color == color && playerCard.Ready)
            {
                return false;
            }
        }

        return true;
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
