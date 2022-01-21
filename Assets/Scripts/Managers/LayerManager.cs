namespace KdClistenysPlatform.Managers
{
    public static class LayerManager
    {
        private static int PlatformsLayer = 8;
        private static int PlayerLayer = 9;
        private static int OneWayPlatformsLayer = 11;
        private static int ProjectilesLayer = 12;
        private static int EnemiesLayer = 13;
        private static int MovingObjectsLayer = 17;
        private static int MovingPlatformsLayer = 18;

        public static int PlatformsLayerMask = 1 << PlatformsLayer;
        public static int OneWayPlatformsLayerMask = 1 << OneWayPlatformsLayer;
        public static int ProjectilesLayerMask = 1 << ProjectilesLayer;
        public static int PlayerLayerMask = 1 << PlayerLayer;
        public static int EnemiesLayerMask = 1 << EnemiesLayer;
        public static int MovingPlatformsLayerMask = 1 << MovingPlatformsLayer;
        public static int MovingObjectsLayerMask = 1 << MovingObjectsLayer;

        public static int ObstaclesLayerMask = LayerManager.PlatformsLayerMask | LayerManager.MovingPlatformsLayerMask | LayerManager.OneWayPlatformsLayerMask;
    }
}
