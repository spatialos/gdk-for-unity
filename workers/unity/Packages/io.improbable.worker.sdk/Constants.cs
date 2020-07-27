namespace Improbable.Worker.CInterop
{
    internal static class Constants
    {
#if UNITY_IOS
        public const string WorkerLibrary = "__Internal";
#else
        public const string WorkerLibrary = "improbable_worker";
#endif
    }
}
