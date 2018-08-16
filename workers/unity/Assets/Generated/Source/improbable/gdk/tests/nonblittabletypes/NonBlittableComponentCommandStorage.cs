// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public class CommandStorages
        {
            public class FirstCommand : CommandStorage
            {
                public Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.FirstCommandRequest>>();
            }
            public class SecondCommand : CommandStorage
            {
                public Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.NonblittableTypes.SecondCommandRequest>>();
            }
        }
    }
}
