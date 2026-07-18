using UnityEngine;

public static class Globals
{
    public static int MaxPoints(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2:
            case 3:
                return 8;
            case 4:
                return 12;
        }
        return 12;
    }

    public static int PointWidth(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2:
            case 3:
                return 18;
            case 4:
                return 12;
        }
        return 12;
    }

    public static int StartPoints(int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2:
            case 3:
                return 4;
            case 4:
                return 6;
        }
        return 6;
    }

    public static float CameraWeight(int position)
    {
        switch (position)
        {
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 3;
            case 4:
                return 4;
        }

        return 1;
    }

    public static Color GetPlayerColor(PlayerColor playerColor, byte alpha)
    {
        switch (playerColor)
        {
            case PlayerColor.Green:
                return new Color32(51, 156, 107, alpha);
            case PlayerColor.Red:
                return new Color32(250, 55, 56, alpha);
            case PlayerColor.Blue:
                return new Color32(35, 163, 229, alpha);
            case PlayerColor.Yellow:
                return new Color32(255, 201, 51, alpha);
            case PlayerColor.Pink:
                return new Color32(249, 69, 127, alpha);
            case PlayerColor.Black:
                return new Color32(10, 5, 5, alpha);
            case PlayerColor.Purple:
                return new Color32(101, 19, 143, alpha);
        }

        return new Color32(255, 255, 255, 255);
    }
}