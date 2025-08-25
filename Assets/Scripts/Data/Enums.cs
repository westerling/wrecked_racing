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
    RocketLauncher = 0,
    Mine = 1,
    DrumBombs = 2,
    Rifle = 3,
    Shotgun = 4,
    OilSlick = 5,
    Boost = 6,
    Flamethrower = 7,
    Mortar = 8,
    Barricades = 9,
}

public enum AmmunitionType
{
    HomingMissile = 0,
    DummyMissile = 1,
    Mine = 2,
    Drumbomb = 3,
    Oil = 4,
    Flames = 5,
    Mortar = 6,
    Barricades = 7,
    Bullet = 8
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

public enum CarStatus
{
    Inactive = 0,
    Active = 1
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

public enum ParticleType
{
    Explosion_s = 0,
    Explosion_m = 1,
    Explosion_l = 2,
    Fire = 3,
    Smoke = 4,
    SkidMarks = 5,
    RifleTrail = 6,
    MuzzleFlash = 7,
    HitEffect = 8
}

public enum SoundFxType
{
    Explosion = 0,
    Fire = 1,
    Start_Stay = 2,
    Start_Go = 3,
    Shotgun = 4,
    Rifle = 5
}