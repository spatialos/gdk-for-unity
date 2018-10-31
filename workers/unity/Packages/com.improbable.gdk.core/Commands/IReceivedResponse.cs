using System.Collections;
using System.Collections.Generic;
using Improbable.Worker;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.Core.Commands
{
    public interface IReceivedResponse
    {
        EntityId EntityId { get; }
        string Message { get; }
        StatusCode StatusCode { get; }
        long RequestId { get; }
    }
}


