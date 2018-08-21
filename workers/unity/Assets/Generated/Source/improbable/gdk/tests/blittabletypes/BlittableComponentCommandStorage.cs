// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public class CommandStorages
        {
            public class FirstCommand : CommandStorage
            {
                public Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>>();
            }
            public class SecondCommand : CommandStorage
            {
                public Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<uint, CommandRequestStore<global::Generated.Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>>();
            }
        }
    }
}
