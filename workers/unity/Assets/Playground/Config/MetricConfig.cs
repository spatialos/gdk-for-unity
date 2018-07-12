namespace Playground
{
    public delegate float CalculateLoadDelegate(float fps);

    public static class MetricConfig
    {
        public static CalculateLoadDelegate CalculateLoad;

        public static int MaxFpsSamples = 50;
        public static float TimeBetweenMetricUpdatesSecs = 2.0f;
    }
}
