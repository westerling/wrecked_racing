public static class LayerMasks
{

    private const int DefaultLayer = 1 << 0;
    private const int WaterLayer = 1 << 4;
    private const int SlipstreamLayer = 1 << 6;
    private const int CarLayer = 1 << 7;
    private const int DriveableLayer = 1 << 8;
    private const int WheelColliderLayer = 1 << 9;
    private const int CheckPointLayer = 1 << 10;
    private const int SpawnPointsLayer = 1 << 11;

    public static readonly int TargeterLayerMask = DefaultLayer | WaterLayer | CarLayer | DriveableLayer;
    public static readonly int SlipstreamLayerMask = SlipstreamLayer;
    public static readonly int CarLayerMask = CarLayer;
    public static readonly int CheckpointsLayerMask = CheckPointLayer | SpawnPointsLayer;
    public static readonly int WheelLayerMask = WheelColliderLayer;
    public static readonly int ShootableLayerMask = DefaultLayer | WaterLayer | CarLayer | DriveableLayer | WheelColliderLayer;
    public static readonly int DriveableLayerMask = DriveableLayer;
    public static readonly int ExplosionLayerMask = DriveableLayer;
}
