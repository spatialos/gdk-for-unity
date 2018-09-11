using System;

namespace Improbable.Gdk.GameObjectRepresentation
{
    /// <summary>
    ///     Enum to note what kind of Injectable a specific type is, to be used as part of the InjectableId.
    /// </summary>
    public enum InjectableType
    {
        ReaderWriter,
        CommandRequestSender,
        CommandRequestHandler,
        CommandResponseHandler,
        WorldCommandRequestSender,
        WorldCommandResponseHandler,
    }

    /// <summary>
    ///     Identifier for specific injectable type, to be used for finding IInjectableCreators and querying from
    ///     the InjectableStore.
    /// </summary>
    public struct InjectableId
    {
        public readonly InjectableType type;
        public readonly uint componentId;

        public InjectableId(InjectableType type, uint componentId)
        {
            this.type = type;
            this.componentId = componentId;
        }

        /// <summary>
        ///     This ID can be used to specify that an injectable is not tied to any components.
        /// </summary>
        public const uint NullComponentId = 0;
    }
}
