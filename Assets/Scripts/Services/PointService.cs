public class PointService : IPointService
{
    public PointService()
    {
    }

    public int CalculatePoints(int position, int currentPoints, int numberOfPlayers)
    {
        switch (numberOfPlayers)
        {
            case 2:
                return CalculatePointsTwoPlayers(position, currentPoints);
            case 3:
                return CalculatePointsThreePlayers(position, currentPoints);
            case 4:
                return CalculatePointsFourPlayers(position, currentPoints);
        }

        return 0;
    }

    private int CalculatePointsTwoPlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 1:
                return 1;
            case 2:
                return currentPoints == 0 ? 0 : -1;
        }

        return 0;
    }

    private int CalculatePointsThreePlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 1:
                return 1;
            case 2:
                return 0;
            case 3:
                return currentPoints == 0 ? 0 : -1;
        }

        return 0;
    }

    private int CalculatePointsFourPlayers(int position, int currentPoints)
    {
        switch (position)
        {
            case 1:
                return currentPoints == 9 ? 1 : 2;
            case 2:
                return currentPoints == 9 ? 0 : 1;
            case 3:
                return currentPoints == 0 ? 0 : -1;
            case 4:
                switch (currentPoints)
                {
                    case 0:
                        return 0;
                    case 1:
                        return -1;
                }
                return -2;
        }

        return 0;
    }
}
