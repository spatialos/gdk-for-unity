using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Worker.CInterop.Query;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

[WorkerType("UnityClient")]
public class MakeSillyQuery : MonoBehaviour
{
    [Require] private WorldCommandSender sender;

    private void OnEnable()
    {
        var nearTheOriginConstraint = new AndConstraint(new IConstraint[]
        {
            new ComponentConstraint(Position.ComponentId),
            new SphereConstraint(0.0, 0.0, 0.0, 50.0)
        });

        var query = new EntityQuery
        {
            ResultType = new SnapshotResultType(new List<uint> { Position.ComponentId }),
            Constraint = nearTheOriginConstraint
        };

        sender.SendEntityQueryCommand(new WorldCommands.EntityQuery.Request(query), HandleQuery);

        //handler.OnEntityQueryResponse += HandleQuery;
    }

    private void HandleQuery(WorldCommands.EntityQuery.ReceivedResponse response)
    {
        foreach (var entity in response.Result)
        {
            Debug.Log(entity.Value.GetComponentSnapshot<Position.Snapshot>().Value.Coords.X);
        }
    }
}
