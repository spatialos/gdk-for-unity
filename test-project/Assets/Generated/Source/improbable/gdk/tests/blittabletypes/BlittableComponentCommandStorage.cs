// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Improbable.Gdk.Core.Commands;

namespace Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        internal class CommandStorages
        {
            public class FirstCommand : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.BlittableTypes.FirstCommandRequest>>();
            }
            public class SecondCommand : CommandStorage
            {
                public Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>> CommandRequestsInFlight =
                    new Dictionary<long, CommandRequestStore<global::Improbable.Gdk.Tests.BlittableTypes.SecondCommandRequest>>();
            }
        }
    }
}
