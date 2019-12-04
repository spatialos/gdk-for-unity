namespace Improbable.Gdk.BuildSystem.Configuration
{
    /// <summary>
    /// Represents a group of build targets and settings.
    /// </summary>
    public enum BuildEnvironment
    {
        /// <summary>
        /// Build targets for local iteration.
        /// </summary>
        Local,

        /// <summary>
        /// Build targets to use for cloud deployment and distribution to players.
        /// </summary>
        Cloud
    }
}
