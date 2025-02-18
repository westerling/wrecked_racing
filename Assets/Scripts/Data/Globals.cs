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
}