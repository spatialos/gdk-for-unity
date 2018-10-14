namespace Improbable.Gdk.Core
{
    /// <summary>
    /// Helpers for defining read and write access for components
    /// </summary>
    public class WorkerRequirements
    {
        /// <summary>
        /// Creates a worker attribute string matching a worker Id
        /// </summary>
        /// <param name="workerId">A worker identifier string</param>
        /// <returns>A worker attribute string matching the given <paramref name="workerId"/></returns>
        public static string IdEquals(string workerId)
        {
            return $"workerId:{workerId}";
        }
    }
}
