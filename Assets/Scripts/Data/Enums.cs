public enum WheelPlacement
{
    FrontLeft = 0,
    FrontRight = 1,
    RearLeft = 2,
    RearRight = 3
}

public enum Drivetrain
{
    FWD = 0,
    RWD = 1,
    AWD = 2
}

public enum CarDirection
{
    Stationary = 0,
    Forward = 1,
    Backward = 2
}

public enum WeaponType
{
    Missile = 0,
    Mine = 1,
    DrumBomb = 2,
    Rifle = 3,
    Shotgun = 4,
    Oil = 5,
    Boost = 6
}

public enum Stat
{
    Speed = 0,
    Acceleration = 1,
    Grip = 2,
    Steer = 3
}

public enum GameState
{
    Menu = 0,
    Race = 1,
    Loading = 3
}

public enum RaceStatus
{
    Countdown = 0,
    Race = 1,
    HeatEnd = 2,
    Finished = 3
}

public enum CountdownEvents
{
    Preparation = 0,
    RedLights = 1,
    YellowLights = 2,
    Start = 3,
    LightsOut = 4
}

public enum SceneIndexes
{
    Manager = 0,
    Title_Screen = 1
}

public enum MenuNavigation
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public enum PlayerStatus
{
    Inactive = 0,
    Active = 1,
    Empty = 2
}

public enum Screens
{
    SplashScreen = 0,
    LoadingScreen = 1,
    PauseScreen = 2,
    PointScreen = 3
}

public enum PlayerColor
{
    Green = 0,
    Red = 1,
    Blyue = 2,
    Yellow = 3
}