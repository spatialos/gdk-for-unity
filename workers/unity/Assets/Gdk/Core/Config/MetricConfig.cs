namespace Improbable.Gdk.Core.Config
{

    public delegate float CalculateLoadDelegate(float);

    public static class MetricConfig
    {
        public static CalculateLoadDelegate CalculateLoad;

        public static int MaxFpsSamples = 50;
        public static int TimeBetweenMetricUpdatesSecs = 2;
    }
}
