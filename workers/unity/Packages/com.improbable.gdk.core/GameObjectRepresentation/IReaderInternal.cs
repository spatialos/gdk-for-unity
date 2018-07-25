﻿using System.Runtime.CompilerServices;
using Improbable.Worker;

[assembly: InternalsVisibleTo("Improbable.Gdk.Core.EditmodeTests")]

namespace Improbable.Gdk.Core.MonoBehaviours
{
    /// <summary>
    ///     The interface that is used to signal a reader that a message has been received from the SpatialOS Dispatcher
    ///     about the component that the reader is connected to.
    /// </summary>
    internal interface IReaderInternal
    {
        void OnAuthorityChange(Authority authority);
        void OnComponentUpdate();
        void OnEvent(int eventIndex);
        void OnCommandRequest(int commandIndex);
    }
}
