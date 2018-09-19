// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        internal class CommandStorages
        {
            public class FirstCommand : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>>();
            }
            public class SecondCommand : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>>();
            }
        }
    }
}
